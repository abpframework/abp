using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class NpmPackagesUpdater : ITransientDependency
    {
        public ILogger<NpmPackagesUpdater> Logger { get; set; }

        private readonly PackageJsonFileFinder _packageJsonFileFinder;

        private readonly Dictionary<string, string> _fileVersionStorage = new Dictionary<string, string>();

        public NpmPackagesUpdater(PackageJsonFileFinder packageJsonFileFinder)
        {
            _packageJsonFileFinder = packageJsonFileFinder;

            Logger = NullLogger<NpmPackagesUpdater>.Instance;
        }

        public void Update(string rootDirectory)
        {
            var fileList = _packageJsonFileFinder.Find(rootDirectory);

            foreach (var file in fileList)
            {
                UpdatePackagesInFile(file);

                RunYarnAndGulp(file);
            }
        }

        protected virtual void UpdatePackagesInFile(string file)
        {
            var fileContent = File.ReadAllText(file);
            var packageJson = JObject.Parse(fileContent);
            var abpPackages = GetAbpPackagesFromPackageJson(packageJson);

            if (!abpPackages.Any())
            {
                return;
            }

            foreach (var abpPackage in abpPackages)
            {
                UpdatePackage(file, abpPackage);
            }

            var modifiedFileContent = packageJson.ToString(Formatting.Indented);

            File.WriteAllText(file, modifiedFileContent);
        }

        protected virtual void UpdatePackage(string file, JProperty package)
        {
            var version = GetLatestVersion(package);

            var versionWithPrefix = $"^{version}";

            if (versionWithPrefix == (string)package.Value)
            {
                return;
            }

            package.Value.Replace(versionWithPrefix);

            Logger.LogInformation($"Updated {package.Name} to {version} in {file.Replace(Directory.GetCurrentDirectory(), "")}.");
        }

        protected virtual string GetLatestVersion(JProperty package)
        {
            if (_fileVersionStorage.ContainsKey(package.Name))
            {
                return _fileVersionStorage[package.Name];
            }

            var version = CmdHelper.RunCmdAndGetOutput($"npm show {package.Name} version");
            
            _fileVersionStorage[package.Name] = version;

            return version;
        }

        protected virtual List<JProperty> GetAbpPackagesFromPackageJson(JObject fileObject)
        {
            var dependencies = (JObject)fileObject["dependencies"];
            var properties = dependencies.Properties().ToList();
            var abpPackages = properties.Where(p => p.Name.StartsWith("@abp/") || p.Name.StartsWith("@volo/")).ToList();
            return abpPackages;
        }

        protected virtual void RunYarnAndGulp(string file)
        {
            var fileDirectory = Path.GetDirectoryName(file).EnsureEndsWith(Path.DirectorySeparatorChar);
            RunYarn(fileDirectory);
            Thread.Sleep(500);
            RunGulp(fileDirectory);
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
