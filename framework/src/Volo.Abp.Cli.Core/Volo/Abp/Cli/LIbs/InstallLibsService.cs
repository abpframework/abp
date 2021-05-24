using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Versioning;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.LIbs
{
    public class InstallLibsService : IInstallLibsService, ITransientDependency
    {
        public ILogger<InstallLibsService> Logger { get; set; }

        private readonly IJsonSerializer _jsonSerializer;

        public InstallLibsService(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
            Logger = NullLogger<InstallLibsService>.Instance;
        }

        public async Task InstallLibs(string directory)
        {
            var projectFiles = Directory.GetFiles(directory, "*.csproj");
            if (!projectFiles.Any())
            {
                Logger.LogError("No project file found in the directory.");
                return;
            }

            if (!await CanInstallLibs(directory))
            {
                Logger.LogWarning(
                    "abp install-libs command is available for MVC, Razor Page, and Blazor-Server UI types");
                return;
            }

            if (!IsNpmInstalled())
            {
                Logger.LogWarning("NPM is not installed, visit https://nodejs.org/en/download/ and install NPM");
                return;
            }

            if (IsYarnAvailable())
            {
                RunYarn(directory);
            }
            else
            {
                RunNpmInstall(directory);
            }

            await CleanAndCopyResources(directory);
        }

        private async Task CleanAndCopyResources(string fileDirectory)
        {
            var mappingFiles = Directory.GetFiles(fileDirectory, "abp.resourcemapping.js", SearchOption.AllDirectories);
            var resourceMapping = new ResourceMapping
            {
                Clean = new List<string> {"./wwwroot/libs"}
            };

            foreach (var mappingFile in mappingFiles)
            {
                using (var reader = File.OpenText(mappingFile))
                {
                    var mappingFileContent = await reader.ReadToEndAsync();

                    var mapping = _jsonSerializer.Deserialize<ResourceMapping>(mappingFileContent
                        .Replace("module.exports", string.Empty)
                        .Replace("=", string.Empty).Trim().TrimEnd(';'));

                    mapping.ReplaceAliases();

                    resourceMapping.Clean.AddRange(mapping.Clean);
                    mapping.Aliases.ToList().ForEach(x =>
                    {
                        resourceMapping.Aliases.AddIfNotContains(new KeyValuePair<string, string>(x.Key, x.Value));
                    });
                    mapping.Mappings.ToList().ForEach(x =>
                    {
                        resourceMapping.Mappings.AddIfNotContains(new KeyValuePair<string, string>(x.Key, x.Value));
                    });
                }
            }

            CleanDirsAndFiles(fileDirectory, resourceMapping);
            CopyResourcesToLibs(fileDirectory, resourceMapping);
        }

        private void CopyResourcesToLibs(string fileDirectory, ResourceMapping resourceMapping)
        {
            foreach (var mapping in resourceMapping.Mappings)
            {
                var sourcePath = Path.Combine(fileDirectory, mapping.Key);
                var destPath = Path.Combine(fileDirectory, mapping.Value);

                if (Path.HasExtension(sourcePath) && File.Exists(sourcePath))
                {
                    Directory.CreateDirectory(Path.GetFullPath(destPath));
                    File.Copy(sourcePath, Path.Combine(destPath, Path.GetFileName(sourcePath)), true);
                }
                else
                {
                    var files = Directory.GetFiles(fileDirectory, mapping.Key);

                    Directory.CreateDirectory(Path.GetFullPath(destPath));

                    foreach (var file in files)
                    {
                        File.Copy(file, Path.Combine(destPath, Path.GetFileName(file)), true);
                    }
                }
            }
        }

        private async Task<bool> CanInstallLibs(string fileDirectory)
        {
            var projectFiles = Directory.GetFiles(fileDirectory, "*.csproj");

            using (var reader = File.OpenText(projectFiles[0]))
            {
                var projectFileContent = await reader.ReadToEndAsync();

                return projectFileContent.Contains("Microsoft.NET.Sdk.Web");
            }
        }

        private void CleanDirsAndFiles(string directory, ResourceMapping resourceMapping)
        {
            foreach (var cleanPattern in resourceMapping.Clean)
            {
                CleanDirsAndFiles(directory, cleanPattern);
            }
        }

        private void CleanDirsAndFiles(string directory, string patterns)
        {
            foreach (var file in Directory.GetFiles(directory, patterns))
            {
                File.Delete(file);
            }

            foreach (var dir in Directory.GetDirectories(directory, patterns))
            {
                Directory.Delete(dir, true);
            }
        }

        private void RunNpmInstall(string directory)
        {
            Logger.LogInformation($"Running npm install on {directory}");
            CmdHelper.RunCmd($"cd {directory} && npm install");
        }

        private void RunYarn(string directory)
        {
            Logger.LogInformation($"Running Yarn on {directory}");
            CmdHelper.RunCmd($"cd {directory} && yarn");
        }

        private bool IsNpmInstalled()
        {
            var output = CmdHelper.RunCmdAndGetOutput("npm -v").Trim();
            return SemanticVersion.TryParse(output, out _);
        }

        private bool IsYarnAvailable()
        {
            var output = CmdHelper.RunCmdAndGetOutput("npm list yarn -g").Trim();
            if (output.Contains("empty"))
            {
                return false;
            }

            if (!SemanticVersion.TryParse(output.Substring(output.IndexOf('@') + 1), out var version))
            {
                return false;
            }

            return version > SemanticVersion.Parse("1.20.0");
        }
    }
}
