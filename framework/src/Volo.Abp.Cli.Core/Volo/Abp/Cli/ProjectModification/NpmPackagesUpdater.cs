using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class NpmPackagesUpdater : ITransientDependency
    {
        private readonly LatestNpmPackageVersionFinder _latestNpmPackageVersionFinder;
        private readonly PackageJsonFileFinder _packageJsonFileFinder;

        private readonly Dictionary<string, string> _fileVersionStorage = new Dictionary<string, string>();

        public NpmPackagesUpdater(LatestNpmPackageVersionFinder latestNpmPackageVersionFinder, PackageJsonFileFinder packageJsonFileFinder)
        {
            _latestNpmPackageVersionFinder = latestNpmPackageVersionFinder;
            _packageJsonFileFinder = packageJsonFileFinder;
        }

        public void Update(string rootDirectory)
        {
            var fileList = _packageJsonFileFinder.Find(rootDirectory);

            foreach (var file in fileList)
            {
                ProcessFile(file);
            }
        }

        protected virtual void ProcessFile(string file)
        {
            ModifyFile(file, out var isFileModified);

            if (isFileModified)
            {
                var fileDirectory = Path.GetDirectoryName(file).EnsureEndsWith(Path.DirectorySeparatorChar);
                CmdHelper.RunCmd($"cd {fileDirectory} && yarn");
                Thread.Sleep(500);
                CmdHelper.RunCmd($"cd {fileDirectory} && gulp");
            }
        }

        protected virtual void ModifyFile(string file, out bool modified)
        {
            modified = false;
            var fileContent = File.ReadAllText(file);

            if (!fileContent.Contains("\"@abp/") && !fileContent.Contains("\"@volo/"))
            {
                return;
            }

            var fileObject = JObject.Parse(fileContent);

            var dependencies = (JObject)fileObject["dependencies"];

            var properties = dependencies.Properties();

            foreach (var property in properties)
            {
                if (property.Name.StartsWith("@abp/") || property.Name.StartsWith("@volo/"))
                {
                    var version = "";

                    if (_fileVersionStorage.ContainsKey(property.Name))
                    {
                        version = _fileVersionStorage[property.Name];
                    }
                    else
                    {
                        _fileVersionStorage[property.Name] = version;
                        version = _latestNpmPackageVersionFinder.Find(property.Name);
                    }

                    property.Value.Replace($"^{version}");
                    modified = true;
                    Console.WriteLine($"Updated {property.Name} to {version} in {file}.");
                }
            }

            var modifiedFileContent = fileObject.ToString(Formatting.Indented);

            File.WriteAllText(file, modifiedFileContent);
        }

    }
}
