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

namespace Volo.Abp.Cli.ProjectModification
{
    public class NpmPackagesUpdater : ITransientDependency
    {
        public ILogger<NpmPackagesUpdater> Logger { get; set; }

        private readonly PackageJsonFileFinder _packageJsonFileFinder;
        private readonly NpmGlobalPackagesChecker _npmGlobalPackagesChecker;
        private readonly MyGetPackageListFinder _myGetPackageListFinder;

        private readonly Dictionary<string, string> _fileVersionStorage = new Dictionary<string, string>();

        public NpmPackagesUpdater(PackageJsonFileFinder packageJsonFileFinder, NpmGlobalPackagesChecker npmGlobalPackagesChecker, MyGetPackageListFinder myGetPackageListFinder)
        {
            _packageJsonFileFinder = packageJsonFileFinder;
            _npmGlobalPackagesChecker = npmGlobalPackagesChecker;
            _myGetPackageListFinder = myGetPackageListFinder;

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
                        await CreateNpmrcFileAsync(Path.GetDirectoryName(file));
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

        private async Task CreateNpmrcFileAsync(string directoryName)
        {
            var fileName = Path.Combine(directoryName, ".npmrc");

            var abpRegistry = "@abp:registry:https://www.myget.org/F/abp-nightly/npm";
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
                    var responseMessage = await client.GetAsync(
                        $"{CliUrls.WwwAbpIo}api/myget/apikey/"
                    );

                    return Encoding.Default.GetString(await responseMessage.Content.ReadAsByteArrayAsync());
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        private bool IsAngularProject(string fileDirectory)
        {
            return File.Exists(Path.Combine(fileDirectory, "angular.json"));
        }

        protected virtual async Task<bool> UpdatePackagesInFile(string file, bool includePreviews = false, bool switchToStable = false)
        {
            var packagesUpdated = false;
            var fileContent = File.ReadAllText(file);
            var packageJson = JObject.Parse(fileContent);
            var abpPackages = GetAbpPackagesFromPackageJson(packageJson);

            if (!abpPackages.Any())
            {
                return packagesUpdated;
            }

            foreach (var abpPackage in abpPackages)
            {
                var updated = await TryUpdatePackage(file, abpPackage, includePreviews, switchToStable);

                if (updated)
                {
                    packagesUpdated = true;
                }
            }

            var modifiedFileContent = packageJson.ToString(Formatting.Indented);

            File.WriteAllText(file, modifiedFileContent);

            return packagesUpdated;
        }

        protected virtual async Task<bool> TryUpdatePackage(string file, JProperty package,
            bool includePreviews = false, bool switchToStable = false)
        {
            var currentVersion = (string)package.Value;

            var version = await GetLatestVersion(package, currentVersion, includePreviews, switchToStable);

            var versionWithPrefix = $"~{version}";

            if (versionWithPrefix == currentVersion)
            {
                return false;
            }

            package.Value.Replace(versionWithPrefix);

            Logger.LogInformation($"Updated {package.Name} to {version} in {file.Replace(Directory.GetCurrentDirectory(), "")}.");
            return true;
        }

        protected virtual async Task<string> GetLatestVersion(JProperty package, string currentVersion,
            bool includePreviews = false, bool switchToStable = false)
        {
            if (_fileVersionStorage.ContainsKey(package.Name))
            {
                return _fileVersionStorage[package.Name];
            }

            string newVersion = currentVersion;

            if (includePreviews || (!switchToStable && currentVersion.Contains("-preview")))
            {
                var mygetPackage = (await _myGetPackageListFinder.GetPackages()).Packages.FirstOrDefault(p => p.Id == package.Name);
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
