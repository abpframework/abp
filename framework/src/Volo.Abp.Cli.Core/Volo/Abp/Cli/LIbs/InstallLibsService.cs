using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        Logger = NullLogger<InstallLibsService>.Instance;
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

                    if (frameworkType == JavaScriptFrameworkType.React)
                    {
                        // React projects are installed with the package manager they declare (the "packageManager"
                        // field or a lock file), so no second lock file is created. They get npm when they
                        // declare none: the React UI and its documentation are based on npm.
                        NpmHelper.RunInstall(projectDirectory);
                    }
                    else
                    {
                        NpmHelper.RunYarn(projectDirectory);
                    }
                }
            }
        }
    }

    private JavaScriptFrameworkType DetectFrameworkTypeFromPackageJson(string packageJsonFilePath)
    {
        try
        {
            return JavaScriptFrameworkDetector.Detect(packageJsonFilePath);
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
        // Shallowest first: when a package is installed more than once, the copy closest to the root wins.
        var mappingFiles = Directory.GetFiles(fileDirectory, "abp.resourcemapping.js", SearchOption.AllDirectories)
            .OrderBy(x => x.Count(c => c == Path.DirectorySeparatorChar))
            .ThenBy(x => x, StringComparer.Ordinal)
            .ToArray();
        var directDependencies = ReadDirectDependencies(fileDirectory);
        var resourceMapping = new ResourceMapping
        {
            Clean = new List<string> { LibsDirectory }
        };
        var addedMappings = new HashSet<KeyValuePair<string, string>>();
        var copyMappings = new List<KeyValuePair<string, string>>();
        var packageDirectories = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var mappingFile in mappingFiles)
        {
            var package = FindPackage(fileDirectory, mappingFile);
            if (package != null)
            {
                // Only the copy closest to the root is used when a package is installed more than once.
                if (packageDirectories.TryGetValue(package.Value.Name, out var packageDirectory) &&
                    packageDirectory != package.Value.Directory)
                {
                    continue;
                }

                packageDirectories[package.Value.Name] = package.Value.Directory;
            }

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
                foreach (var x in mapping.Mappings)
                {
                    // A mapping that several files declare is copied once, from the shallowest file that has files for it.
                    if (addedMappings.Contains(x))
                    {
                        continue;
                    }

                    var resolvedPattern = ResolveDependencyPath(fileDirectory, package?.Directory, directDependencies, x.Key);
                    if (FindFiles(fileDirectory, resolvedPattern).Any())
                    {
                        addedMappings.Add(x);
                    }

                    copyMappings.AddIfNotContains(new KeyValuePair<string, string>(resolvedPattern, x.Value));
                }
            }
        }

        EnsureLibsFolderExists(fileDirectory, LibsDirectory);

        CleanDirsAndFiles(fileDirectory, resourceMapping);
        CopyResourcesToLibs(fileDirectory, copyMappings);
    }

    private HashSet<string> ReadDirectDependencies(string projectDirectory)
    {
        var dependencies = new HashSet<string>(StringComparer.Ordinal);
        var packageJsonFilePath = Path.Combine(projectDirectory, "package.json");

        try
        {
            var packageJson = JObject.Parse(File.ReadAllText(packageJsonFilePath));
            foreach (var dependencyListName in new[] { "dependencies", "devDependencies", "optionalDependencies" })
            {
                if (packageJson[dependencyListName] is JObject dependencyList)
                {
                    foreach (var dependency in dependencyList.Properties())
                    {
                        dependencies.Add(dependency.Name);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to parse package.json at {PackageJsonFilePath}", packageJsonFilePath);
        }

        return dependencies;
    }

    // Returns the npm package that contains the mapping file, or null for a mapping file of the project itself.
    private static (string Name, string Directory)? FindPackage(string projectDirectory, string mappingFile)
    {
        var root = Path.GetFullPath(projectDirectory).TrimEnd(Path.DirectorySeparatorChar);
        var segments = Path.GetFullPath(mappingFile).Substring(root.Length + 1).Split(Path.DirectorySeparatorChar);
        var index = Array.FindLastIndex(segments, x => x.Equals("node_modules", StringComparison.OrdinalIgnoreCase));
        if (index < 0)
        {
            return null;
        }

        var nameSegmentCount = segments[index + 1].StartsWith("@") ? 2 : 1;
        if (index + nameSegmentCount >= segments.Length - 1)
        {
            return null;
        }

        var directorySegments = segments.Take(index + 1 + nameSegmentCount).ToList();
        return (
            string.Join("/", directorySegments.Skip(index + 1)),
            Path.Combine(root, Path.Combine(directorySegments.ToArray())));
    }

    // When the root node_modules has another version of a package's dependency, the package manager nests the
    // version the package needs under the package's own node_modules. Resolve it the way Node.js does, except for
    // the direct dependencies of the project: their version in the root node_modules is the one the project chose.
    // A nested copy that has no file for the pattern is skipped, so the root copy is used as before. The mapping
    // files of the project itself (packageDirectory is null) keep using the root node_modules.
    private string ResolveDependencyPath(string projectDirectory, string packageDirectory, HashSet<string> directDependencies, string pattern)
    {
        const string nodeModulesPrefix = "./node_modules/";
        if (packageDirectory == null)
        {
            return pattern;
        }

        var normalizedPattern = pattern.Replace('\\', '/');
        if (normalizedPattern.StartsWith("node_modules/"))
        {
            normalizedPattern = "./" + normalizedPattern;
        }

        if (!normalizedPattern.StartsWith(nodeModulesPrefix))
        {
            return pattern;
        }

        var path = normalizedPattern.Substring(nodeModulesPrefix.Length);
        var segments = path.Split('/');
        var nameSegmentCount = segments[0].StartsWith("@") ? 2 : 1;
        if (segments.Length < nameSegmentCount || segments.Take(nameSegmentCount).Any(x => x.Contains("*")))
        {
            return pattern;
        }

        var packageName = string.Join("/", segments.Take(nameSegmentCount));
        var root = Path.GetFullPath(projectDirectory).TrimEnd(Path.DirectorySeparatorChar);
        if (directDependencies.Contains(packageName) && Directory.Exists(Path.Combine(root, "node_modules", packageName)))
        {
            return pattern;
        }

        var directory = Path.GetFullPath(packageDirectory).TrimEnd(Path.DirectorySeparatorChar);

        while (directory != null &&
               directory.Length > root.Length &&
               directory.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
        {
            if (!Path.GetFileName(directory).Equals("node_modules", StringComparison.OrdinalIgnoreCase) &&
                Directory.Exists(Path.Combine(directory, "node_modules", packageName)))
            {
                var relativeDirectory = directory.Substring(root.Length + 1).Replace('\\', '/');
                var resolvedPattern = $"./{relativeDirectory}/node_modules/{path}";
                if (FindFiles(projectDirectory, resolvedPattern).Any())
                {
                    return resolvedPattern;
                }
            }

            directory = Path.GetDirectoryName(directory);
        }

        return pattern;
    }

    private void EnsureLibsFolderExists(string fileDirectory, string libsDirectory)
    {
        Directory.CreateDirectory(Path.Combine(fileDirectory, libsDirectory));
    }

    private void CopyResourcesToLibs(string fileDirectory, List<KeyValuePair<string, string>> mappings)
    {
        foreach (var mapping in mappings)
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
