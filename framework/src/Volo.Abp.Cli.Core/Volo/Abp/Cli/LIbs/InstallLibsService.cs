﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
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
        public const string LibsDirectory = "./wwwroot/libs";

        public ILogger<InstallLibsService> Logger { get; set; }

        private readonly IJsonSerializer _jsonSerializer;

        public InstallLibsService(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
            Logger = NullLogger<InstallLibsService>.Instance;
        }

        public async Task InstallLibsAsync(string directory)
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
                Clean = new List<string> {LibsDirectory}
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

                    mapping.Clean.ForEach(c => resourceMapping.Clean.AddIfNotContains(c));
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

            EnsureLibsFolderExists(fileDirectory, LibsDirectory);

            CleanDirsAndFiles(fileDirectory, resourceMapping);
            CopyResourcesToLibs(fileDirectory, resourceMapping);
        }

        private void EnsureLibsFolderExists(string fileDirectory, string libsDirectory)
        {
            Directory.CreateDirectory(Path.Combine(fileDirectory, libsDirectory));
        }

        private void CopyResourcesToLibs(string fileDirectory, ResourceMapping resourceMapping)
        {
            foreach (var mapping in resourceMapping.Mappings)
            {
                var destPath = Path.Combine(fileDirectory, mapping.Value);
                var files = FindFiles(fileDirectory, mapping.Key);

                foreach (var file in files)
                {
                    var destFilePath = Path.Combine(destPath, file.Stem);
                    if (File.Exists(destFilePath))
                    {
                        continue;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(destFilePath));
                    File.Copy(file.Path, destFilePath);

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
            var files = FindFiles(directory, resourceMapping.Clean.ToArray());

            foreach (var file in files)
            {
                if (File.Exists(file.Path))
                {
                    File.Delete(file.Path);
                }
            }

            foreach (var directoryInfo in Directory.GetDirectories(Path.Combine(directory, resourceMapping.Clean.First()),"*", SearchOption.AllDirectories).Reverse())
            {
                if (!Directory.EnumerateFileSystemEntries(directoryInfo).Any())
                {
                    Directory.Delete(directoryInfo);
                }
            }
        }

        private List<FileMatchResult> FindFiles(string directory, params string[] patterns)
        {
            var matcher = new Matcher();

            foreach (var pattern in patterns)
            {
                if (pattern.StartsWith("!"))
                {
                    matcher.AddExclude(NormalizeGlob(pattern).TrimStart('!'));
                }
                else
                {
                    matcher.AddInclude(NormalizeGlob(pattern));
                }
            }

            var result = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(directory)));

            return result.Files.Select(x => new FileMatchResult(Path.Combine(directory, x.Path), x.Stem)).ToList();
        }

        private string NormalizeGlob(string pattern)
        {
            pattern = pattern.Replace("//", "/");

            if (!Path.HasExtension(pattern) && !pattern.EndsWith("*"))
            {
                return pattern.EnsureEndsWith('/') + "**";
            }

            return pattern;
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
