using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using NuGet.Versioning;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.LIbs;

public class InstallLibsService : IInstallLibsService, ITransientDependency
{
    private readonly static List<string> ExcludeDirectory = new List<string>()
    {
        "node_modules",
        ".git",
        ".idea",
        Path.Combine("bin", "debug"),
        Path.Combine("obj", "debug")
    };

    public NpmHelper NpmHelper { get; }
    public const string LibsDirectory = "./wwwroot/libs";

    public ILogger<InstallLibsService> Logger { get; set; }

    public InstallLibsService(NpmHelper npmHelper)
    {
        NpmHelper = npmHelper;
    }

    public async Task InstallLibsAsync(string directory)
    {
        var projectPaths = FindAllProjects(directory);
        if (!projectPaths.Any())
        {
            Logger.LogError("No project found in the directory.");
            return;
        }

        if (!NpmHelper.IsNpmInstalled())
        {
            Logger.LogWarning("NPM is not installed, visit https://nodejs.org/en/download/ and install NPM");
            return;
        }

        if (!NpmHelper.IsYarnAvailable())
        {
            Logger.LogWarning("YARN is not installed, which may cause package inconsistency, please use YARN instead of NPM. visit https://classic.yarnpkg.com/lang/en/docs/install/ and install YARN");
        }

        Logger.LogInformation($"Found {projectPaths.Count} projects.");
        foreach (var projectPath in projectPaths)
        {
            Logger.LogInformation($"{Path.GetDirectoryName(projectPath)}");
        }

        foreach (var projectPath in projectPaths)
        {
            var projectDirectory = Path.GetDirectoryName(projectPath);

            // angular
            if (projectPath.EndsWith("angular.json"))
            {
                if (NpmHelper.IsYarnAvailable())
                {
                    NpmHelper.RunYarn(projectDirectory);
                }
                else
                {
                    NpmHelper.RunNpmInstall(projectDirectory);
                }
            }

            // MVC or BLAZOR SERVER
            if (projectPath.EndsWith(".csproj"))
            {
                var packageJsonFilePath = Path.Combine(Path.GetDirectoryName(projectPath), "package.json");

                if (!File.Exists(packageJsonFilePath))
                {
                    continue;
                }

                if (NpmHelper.IsYarnAvailable())
                {
                    NpmHelper.RunYarn(projectDirectory);
                }
                else
                {
                    NpmHelper.RunNpmInstall(projectDirectory);
                }

                await CleanAndCopyResources(projectDirectory);
            }
        }
    }

    private List<string> FindAllProjects(string directory)
    {
        return Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories)
            .Union(Directory.GetFiles(directory, "angular.json", SearchOption.AllDirectories))
            .Where(file => ExcludeDirectory.All(x => file.IndexOf(x, StringComparison.OrdinalIgnoreCase) == -1))
            .Where(file =>
            {
                if (file.EndsWith(".csproj"))
                {
                    var packageJsonFilePath = Path.Combine(Path.GetDirectoryName(file), "package.json");
                    if (!File.Exists(packageJsonFilePath))
                    {
                        return false;
                    }

                    using (var reader = File.OpenText(file))
                    {
                        return reader.ReadToEnd().Contains("Microsoft.NET.Sdk.Web");
                    }
                }
                return true;
            })
            .OrderBy(x => x)
            .ToList();
    }

    private async Task CleanAndCopyResources(string fileDirectory)
    {
        var mappingFiles = Directory.GetFiles(fileDirectory, "abp.resourcemapping.js", SearchOption.AllDirectories);
        var resourceMapping = new ResourceMapping
        {
            Clean = new List<string> { LibsDirectory }
        };

        foreach (var mappingFile in mappingFiles)
        {
            using (var reader = File.OpenText(mappingFile))
            {
                var mappingFileContent = await reader.ReadToEndAsync();

                // System.Text.Json doesn't support the property name without quotes.
                var mapping = Newtonsoft.Json.JsonConvert.DeserializeObject<ResourceMapping>(mappingFileContent
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

        foreach (var directoryInfo in Directory.GetDirectories(Path.Combine(directory, resourceMapping.Clean.First()), "*", SearchOption.AllDirectories).Reverse())
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
}
