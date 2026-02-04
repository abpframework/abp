using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.LIbs;

public enum JavaScriptFrameworkType
{
    None,
    ReactNative,
    React,
    Vue,
    NextJs
}

public class InstallLibsService : IInstallLibsService, ITransientDependency
{
    private readonly static List<string> ExcludeDirectory = new List<string>()
    {
        "node_modules",
        ".git",
        ".idea",
        "_templates",
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
                NpmHelper.RunYarn(projectDirectory);
            }

            // MVC or BLAZOR SERVER
            if (projectPath.EndsWith(".csproj"))
            {
                var packageJsonFilePath = Path.Combine(Path.GetDirectoryName(projectPath), "package.json");

                if (!File.Exists(packageJsonFilePath))
                {
                    continue;
                }

                NpmHelper.RunYarn(projectDirectory);

                await CleanAndCopyResources(projectDirectory);
            }

            // JavaScript frameworks (React Native, React, Vue, Next.js)
            if (projectPath.EndsWith("package.json"))
            {
                var frameworkType = DetectFrameworkTypeFromPackageJson(projectPath);

                if (frameworkType != JavaScriptFrameworkType.None)
                {
                    var frameworkName = frameworkType switch
                    {
                        JavaScriptFrameworkType.ReactNative => "React Native",
                        JavaScriptFrameworkType.React => "React",
                        JavaScriptFrameworkType.Vue => "Vue.js",
                        JavaScriptFrameworkType.NextJs => "Next.js",
                        _ => "JavaScript"
                    };

                    Logger.LogInformation($"Installing dependencies for {frameworkName} project: {projectDirectory}");
                    NpmHelper.RunYarn(projectDirectory);
                }
            }
        }
    }

    private JavaScriptFrameworkType DetectFrameworkTypeFromPackageJson(string packageJsonFilePath)
    {
        if (!File.Exists(packageJsonFilePath))
        {
            return JavaScriptFrameworkType.None;
        }

        try
        {
            var packageJsonContent = File.ReadAllText(packageJsonFilePath);
            var packageJson = JObject.Parse(packageJsonContent);

            var dependencies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            // Check dependencies
            if (packageJson["dependencies"] is JObject deps)
            {
                foreach (var prop in deps.Properties())
                {
                    dependencies.Add(prop.Name);
                }
            }

            // Check devDependencies
            if (packageJson["devDependencies"] is JObject devDeps)
            {
                foreach (var prop in devDeps.Properties())
                {
                    dependencies.Add(prop.Name);
                }
            }

            // Check for React Native first (has priority over React)
            if (dependencies.Contains("react-native"))
            {
                return JavaScriptFrameworkType.ReactNative;
            }

            // Check for other frameworks
            if (dependencies.Contains("next"))
            {
                return JavaScriptFrameworkType.NextJs;
            }

            if (dependencies.Contains("vue"))
            {
                return JavaScriptFrameworkType.Vue;
            }

            if (dependencies.Contains("react"))
            {
                return JavaScriptFrameworkType.React;
            }

            return JavaScriptFrameworkType.None;
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to parse package.json at {PackageJsonFilePath}", packageJsonFilePath);
            return JavaScriptFrameworkType.None;
        }
    }

    private List<string> FindAllProjects(string directory)
    {
        var projects = new List<string>();

        // Find .csproj files (existing logic)
        var csprojFiles = Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories)
            .Where(file => ExcludeDirectory.All(x => file.IndexOf(x + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) == -1))
            .Where(file =>
            {
                var packageJsonFilePath = Path.Combine(Path.GetDirectoryName(file), "package.json");
                if (!File.Exists(packageJsonFilePath))
                {
                    return false;
                }

                using (var reader = File.OpenText(file))
                {
                    var fileTexts = reader.ReadToEnd();
                    return fileTexts.Contains("Microsoft.NET.Sdk.Web") ||
                           fileTexts.Contains("Microsoft.NET.Sdk.Razor") ||
                           fileTexts.Contains("Microsoft.NET.Sdk.BlazorWebAssembly");
                }
            });

        projects.AddRange(csprojFiles);

        // Find angular.json files (existing logic)
        var angularFiles = Directory.GetFiles(directory, "angular.json", SearchOption.AllDirectories)
            .Where(file => ExcludeDirectory.All(x => file.IndexOf(x + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) == -1));

        projects.AddRange(angularFiles);

        // Find package.json files for JavaScript frameworks
        var packageJsonFiles = Directory.GetFiles(directory, "package.json", SearchOption.AllDirectories)
            .Where(file => ExcludeDirectory.All(x => file.IndexOf(x + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) == -1))
            .Where(packageJsonFile =>
            {
                var packageJsonDirectory = Path.GetDirectoryName(packageJsonFile);
                if (packageJsonDirectory == null)
                {
                    return false;
                }

                // Skip if already handled by Angular or .NET detection
                if (File.Exists(Path.Combine(packageJsonDirectory, "angular.json")))
                {
                    return false;
                }

                if (Directory.GetFiles(packageJsonDirectory, "*.csproj", SearchOption.TopDirectoryOnly).Any())
                {
                    return false;
                }

                // Check if it's a JavaScript framework project
                var frameworkType = DetectFrameworkTypeFromPackageJson(packageJsonFile);
                return frameworkType != JavaScriptFrameworkType.None;
            });

        projects.AddRange(packageJsonFiles);

        return projects.OrderBy(x => x).ToList();
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

        var directoryInfos = Directory.GetDirectories(Path.Combine(directory, resourceMapping.Clean.First()), "*", SearchOption.AllDirectories);
        directoryInfos.Reverse();
        foreach (var directoryInfo in directoryInfos)
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
