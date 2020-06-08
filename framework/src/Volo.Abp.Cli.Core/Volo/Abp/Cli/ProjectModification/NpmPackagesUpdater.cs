using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectModification
{
    public class NpmPackagesUpdater : ITransientDependency
    {
        public ILogger<NpmPackagesUpdater> Logger { get; set; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        private readonly PackageJsonFileFinder _packageJsonFileFinder;
        private readonly NpmGlobalPackagesChecker _npmGlobalPackagesChecker;
        private readonly MyGetPackageListFinder _myGetPackageListFinder;
        private readonly Dictionary<string, string> _fileVersionStorage = new Dictionary<string, string>();
        private MyGetApiResponse _myGetApiResponse;

        public NpmPackagesUpdater(
            PackageJsonFileFinder packageJsonFileFinder,
            NpmGlobalPackagesChecker npmGlobalPackagesChecker,
            MyGetPackageListFinder myGetPackageListFinder,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            _packageJsonFileFinder = packageJsonFileFinder;
            _npmGlobalPackagesChecker = npmGlobalPackagesChecker;
            _myGetPackageListFinder = myGetPackageListFinder;
            CancellationTokenProvider = cancellationTokenProvider;
            Logger = NullLogger<NpmPackagesUpdater>.Instance;
        }

        public async Task Update(string rootDirectory, bool includePreviews = false, bool switchToStable = false)
        {
            var fileList = _packageJsonFileFinder.Find(rootDirectory);

            if (!fileList.Any())
            {
                return;
            }

            _npmGlobalPackagesChecker.Check();

            foreach (var file in fileList)
            {
                var packagesUpdated = await UpdatePackagesInFile(file, includePreviews, switchToStable);

                if (packagesUpdated)
                {
                    var fileDirectory = Path.GetDirectoryName(file).EnsureEndsWith(Path.DirectorySeparatorChar);

                    if (IsAngularProject(fileDirectory))
                    {
                        if (includePreviews)
                        {
                            await CreateNpmrcFileAsync(Path.GetDirectoryName(file));
                        }
                        else if (switchToStable)
                        {
                            await DeleteNpmrcFileAsync(Path.GetDirectoryName(file));
                        }
                    }

                    RunYarn(fileDirectory);

                    if (!IsAngularProject(fileDirectory))
                    {
                        Thread.Sleep(500);
                        RunGulp(fileDirectory);
                    }
                }
            }
        }

        private static async Task DeleteNpmrcFileAsync(string directoryName)
        {
            FileHelper.DeleteIfExists(Path.Combine(directoryName, ".npmrc"));

            await Task.CompletedTask;
        }

        private async Task CreateNpmrcFileAsync(string directoryName)
        {
            var fileName = Path.Combine(directoryName, ".npmrc");

            var abpRegistry = "@abp:registry=https://www.myget.org/F/abp-nightly/npm";
            var voloRegistry = await GetVoloRegistryAsync();

            if (File.Exists(fileName))
            {
                var fileContent = File.ReadAllText(fileName);

                if (!fileContent.Contains(abpRegistry))
                {
                    fileContent += Environment.NewLine + abpRegistry;
                }

                if (!fileContent.Contains(voloRegistry))
                {
                    fileContent += Environment.NewLine + voloRegistry;
                }

                File.WriteAllText(fileName, fileContent);

                return;
            }

            using var fs = File.Create(fileName);

            var content = new UTF8Encoding(true)
                .GetBytes(abpRegistry + Environment.NewLine + voloRegistry);
            fs.Write(content, 0, content.Length);
        }

        private async Task<string> GetVoloRegistryAsync()
        {
            var apikey = await GetApiKeyAsync();

            if (string.IsNullOrWhiteSpace(apikey))
            {
                return "";
            }

            return "@volo:registry=https://www.myget.org/F/abp-commercial/auth/" + apikey + "/npm/";
        }

        public async Task<string> GetApiKeyAsync()
        {
            try
            {
                using (var client = new CliHttpClient(TimeSpan.FromMinutes(1)))
                {
                    var response = await client.GetHttpResponseMessageWithRetryAsync(
                        url: $"{CliUrls.WwwAbpIo}api/myget/apikey/",
                        cancellationToken: CancellationTokenProvider.Token,
                        logger: Logger
                    );

                    return Encoding.Default.GetString(await response.Content.ReadAsByteArrayAsync());
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static bool IsAngularProject(string fileDirectory)
        {
            return File.Exists(Path.Combine(fileDirectory, "angular.json"));
        }

        protected virtual async Task<bool> UpdatePackagesInFile(string filePath, bool includePreviews = false, bool switchToStable = false)
        {
            var packagesUpdated = false;
            var fileContent = File.ReadAllText(filePath);
            var packageJson = JObject.Parse(fileContent);
            var abpPackages = GetAbpPackagesFromPackageJson(packageJson);

            if (!abpPackages.Any())
            {
                return false;
            }

            foreach (var abpPackage in abpPackages)
            {
                var updated = await TryUpdatingPackage(filePath, abpPackage, includePreviews, switchToStable);

                if (updated)
                {
                    packagesUpdated = true;
                }
            }

            var updatedContent = packageJson.ToString(Formatting.Indented);

            File.WriteAllText(filePath, updatedContent);

            return packagesUpdated;
        }

        protected virtual async Task<bool> TryUpdatingPackage(
            string filePath,
            JProperty package,
            bool includePreviews = false,
            bool switchToStable = false)
        {
            var currentVersion = (string)package.Value;

            var version = await GetLatestVersion(package, currentVersion, includePreviews, switchToStable);

            var versionWithPrefix = $"~{version}";

            if (versionWithPrefix == currentVersion)
            {
                return false;
            }

            package.Value.Replace(versionWithPrefix);

            Logger.LogInformation($"Updated {package.Name} to {version} in {filePath.Replace(Directory.GetCurrentDirectory(), "")}.");
            return true;
        }

        protected virtual async Task<string> GetLatestVersion(
            JProperty package,
            string currentVersion,
            bool includePreviews = false,
            bool switchToStable = false)
        {
            if (_fileVersionStorage.ContainsKey(package.Name))
            {
                return _fileVersionStorage[package.Name];
            }

            var newVersion = currentVersion;

            if (includePreviews || (!switchToStable && currentVersion.Contains("-preview")))
            {
                if (_myGetApiResponse == null)
                {
                    _myGetApiResponse = await _myGetPackageListFinder.GetPackagesAsync();
                }

                var mygetPackage = _myGetApiResponse.Packages.FirstOrDefault(p => p.Id == package.Name);
                if (mygetPackage != null)
                {
                    newVersion = mygetPackage.Versions.Last();
                }
            }
            else
            {
                newVersion = CmdHelper.RunCmdAndGetOutput($"npm show {package.Name} version");
            }

            _fileVersionStorage[package.Name] = newVersion;

            return newVersion;
        }

        protected virtual List<JProperty> GetAbpPackagesFromPackageJson(JObject fileObject)
        {
            var dependencyList = new[] { "dependencies", "devDependencies", "peerDependencies" };
            var abpPackages = new List<JProperty>();

            foreach (var dependencyListName in dependencyList)
            {
                var dependencies = (JObject)fileObject[dependencyListName];

                if (dependencies == null)
                {
                    continue;
                }

                var properties = dependencies.Properties().ToList();
                abpPackages.AddRange(properties.Where(p => p.Name.StartsWith("@abp/") || p.Name.StartsWith("@volo/")).ToList());
            }

            return abpPackages;
        }

        protected virtual void RunGulp(string fileDirectory)
        {
            Logger.LogInformation($"Running Gulp on {fileDirectory}");
            CmdHelper.RunCmd($"cd {fileDirectory} && gulp");
        }

        protected virtual void RunYarn(string fileDirectory)
        {
            Logger.LogInformation($"Running Yarn on {fileDirectory}");
            CmdHelper.RunCmd($"cd {fileDirectory} && yarn");
        }
    }
}
