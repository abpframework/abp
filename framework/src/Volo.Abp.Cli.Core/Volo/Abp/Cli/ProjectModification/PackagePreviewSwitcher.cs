using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class PackagePreviewSwitcher : ITransientDependency
{
    private readonly PackageSourceManager _packageSourceManager;
    private readonly NpmPackagesUpdater _npmPackagesUpdater;
    private readonly VoloNugetPackagesVersionUpdater _nugetPackagesVersionUpdater;

    public ILogger<PackagePreviewSwitcher> Logger { get; set; }

    public PackagePreviewSwitcher(PackageSourceManager packageSourceManager,
        NpmPackagesUpdater npmPackagesUpdater,
        VoloNugetPackagesVersionUpdater nugetPackagesVersionUpdater)
    {
        _packageSourceManager = packageSourceManager;
        _npmPackagesUpdater = npmPackagesUpdater;
        _nugetPackagesVersionUpdater = nugetPackagesVersionUpdater;
        Logger = NullLogger<PackagePreviewSwitcher>.Instance;
    }

    public async Task SwitchToPreview(CommandLineArgs commandLineArgs)
    {
        var solutionPaths = GetSolutionPaths(commandLineArgs);

        if (solutionPaths.Any())
        {
            await SwitchSolutionsToPreview(solutionPaths);
        }
        else
        {
            var projectPaths = GetProjectPaths(commandLineArgs);
            
            await SwitchProjectsToPreview(projectPaths);
        }
    }
    
    public async Task SwitchToStable(CommandLineArgs commandLineArgs)
    {
        var solutionPaths = GetSolutionPaths(commandLineArgs);

        if (solutionPaths.Any())
        {
            await SwitchSolutionsToStable(solutionPaths);
        }
        else
        {
            var projectPaths = GetProjectPaths(commandLineArgs);
            
            await SwitchProjectsToStable(projectPaths);
        }
    }
    
    public async Task SwitchToNightlyPreview(CommandLineArgs commandLineArgs)
    {
        var solutionPaths = GetSolutionPaths(commandLineArgs);

        if (solutionPaths.Any())
        {
            await SwitchSolutionsToNightlyPreview(solutionPaths, commandLineArgs);
        }
        else
        {
            var projectPaths = GetProjectPaths(commandLineArgs);

            await SwitchProjectsToNightlyPreview(projectPaths, commandLineArgs);
        }
    }

    public async Task SwitchToPreRc(CommandLineArgs commandLineArgs)
    {
        var solutionPaths = GetSolutionPaths(commandLineArgs);

        if (solutionPaths.Any())
        {
            await SwitchNpmPackageVersionsOfSolutionsToPreRc(solutionPaths);
        }
        else
        {
            await SwitchNpmPackageVersionsOfProjectsToPreRc(GetProjectPaths(commandLineArgs));
        }
    }

    private async Task SwitchProjectsToPreview(List<string> projects)
    {
        foreach (var project in projects)
        {
            var folder = Path.GetDirectoryName(project);

            await _nugetPackagesVersionUpdater.UpdateProjectAsync(
                project,
                includeReleaseCandidates: true);

            await _npmPackagesUpdater.Update(
                folder,
                false,
                true);
        }
    }

    private async Task SwitchSolutionsToPreview(List<string> solutionPaths)
    {
        foreach (var solutionPath in solutionPaths)
        {
            var solutionFolder = Path.GetDirectoryName(solutionPath);
            var solutionAngularFolder = GetSolutionAngularFolder(solutionFolder);

            await _nugetPackagesVersionUpdater.UpdateSolutionAsync(
                solutionPath,
                includeReleaseCandidates: true);

            await _npmPackagesUpdater.Update(
                solutionFolder,
                false,
                true);

            if (solutionAngularFolder != null)
            {
                await _npmPackagesUpdater.Update(
                    solutionAngularFolder,
                    false,
                    true);
            }
        }
    }

    private async Task SwitchProjectsToStable(List<string> projects)
    {
        foreach (var project in projects)
        {
            var folder = Path.GetDirectoryName(project);

            await _nugetPackagesVersionUpdater.UpdateProjectAsync(
                project,
                false,
                false,
                true);

            await _npmPackagesUpdater.Update(
                folder,
                false,
                false,
                true);
        }
    }

    private async Task SwitchSolutionsToStable(List<string> solutionPaths)
    {
        foreach (var solutionPath in solutionPaths)
        {
            var solutionFolder = Path.GetDirectoryName(solutionPath);
            var solutionAngularFolder = GetSolutionAngularFolder(solutionFolder);

            _packageSourceManager.Remove(solutionFolder, "ABP Nightly");

            await _nugetPackagesVersionUpdater.UpdateSolutionAsync(
                solutionPath,
                false,
                false,
                true);

            await _npmPackagesUpdater.Update(
                solutionFolder,
                false,
                false,
                true);

            if (solutionAngularFolder != null)
            {
                await _npmPackagesUpdater.Update(
                    solutionAngularFolder,
                    false,
                    false,
                    true);
            }
        }
    }

    private async Task SwitchProjectsToNightlyPreview(List<string> projects, CommandLineArgs commandLineArgs)
    {
        var (includeFiles, excludedPackages, latestVersionFromMyGet) = await ResolveNightlyIncludeContextAsync(commandLineArgs);

        foreach (var project in projects)
        {
            var folder = Path.GetDirectoryName(project);
            var projectFolder = FindSolutionFolder(project) ?? folder;

            _packageSourceManager.Add(projectFolder, "ABP Nightly",
                "https://www.myget.org/F/abp-nightly/api/v3/index.json", "Volo.*");

            await _nugetPackagesVersionUpdater.UpdateSolutionAsync(
                project,
                true);

            await _npmPackagesUpdater.Update(
                folder,
                true);

            // See SwitchSolutionsToNightlyPreview for the race-avoidance rationale: this
            // sequential pass always runs after the per-project UpdateSolutionAsync above.
            await UpdateIncludedCentralPackageFilesAsync(includeFiles, excludedPackages, latestVersionFromMyGet, projectFolder);
        }
    }

    private async Task SwitchSolutionsToNightlyPreview(List<string> solutionPaths, CommandLineArgs commandLineArgs)
    {
        var (includeFiles, excludedPackages, latestVersionFromMyGet) = await ResolveNightlyIncludeContextAsync(commandLineArgs);

        foreach (var solutionPath in solutionPaths)
        {
            var solutionFolder = Path.GetDirectoryName(solutionPath);
            var solutionAngularFolder = GetSolutionAngularFolder(solutionFolder);

            _packageSourceManager.Add(solutionFolder, "ABP Nightly",
                "https://www.myget.org/F/abp-nightly/api/v3/index.json",
                "Volo.*");

            if (solutionPath != null)
            {
                await _nugetPackagesVersionUpdater.UpdateSolutionAsync(
                    solutionPath,
                    true);
            }

            await _npmPackagesUpdater.Update(
                solutionFolder,
                true);

            if (solutionAngularFolder != null)
            {
                await _npmPackagesUpdater.Update(
                    solutionAngularFolder,
                    true);
            }

            // Optional Central Package Management support: only runs when --include is
            // explicitly passed, and only after UpdateSolutionAsync's internal parallel
            // (Task.WaitAll) per-project update has fully completed, so no --include file
            // is ever touched concurrently with anything else.
            await UpdateIncludedCentralPackageFilesAsync(includeFiles, excludedPackages, latestVersionFromMyGet, solutionFolder);
        }
    }

    private async Task<(List<string> IncludeFiles, List<string> ExcludedPackages, string LatestVersionFromMyGet)> ResolveNightlyIncludeContextAsync(
        CommandLineArgs commandLineArgs)
    {
        var includeFiles = GetCommaSeparatedOption(commandLineArgs, Options.Include.Short, Options.Include.Long);
        var excludedPackages = GetCommaSeparatedOption(commandLineArgs, Options.Exclude.Short, Options.Exclude.Long);

        if (!includeFiles.Any())
        {
            return (includeFiles, excludedPackages, null);
        }

        string latestVersionFromMyGet;
        try
        {
            latestVersionFromMyGet = await _nugetPackagesVersionUpdater.GetLatestVersionFromMyGet("Volo.Abp.Core");
        }
        catch (Exception ex)
        {
            // Don't let a transient MyGet failure abort the whole switch-to-nightly run
            // (source registration / regular PackageReference updates below must still
            // proceed for every solution/project) - just skip the --include pass.
            Logger.LogWarning(ex, "Could not resolve the latest Volo.Abp.Core nightly version; --include files will be skipped for this run.");
            return (includeFiles, excludedPackages, null);
        }

        if (latestVersionFromMyGet.IsNullOrWhiteSpace())
        {
            // No exception was thrown, but MyGet simply has no version for this package yet
            // (e.g. not indexed there) - warn so users aren't left wondering why --include did nothing.
            Logger.LogWarning("Could not resolve the latest Volo.Abp.Core nightly version; --include files will be skipped for this run.");
            return (includeFiles, excludedPackages, null);
        }

        return (includeFiles, excludedPackages, latestVersionFromMyGet);
    }

    private async Task UpdateIncludedCentralPackageFilesAsync(
        List<string> includeFiles,
        List<string> excludedPackages,
        string latestVersionFromMyGet,
        string baseFolder)
    {
        foreach (var includeFile in includeFiles)
        {
            var resolvedPath = Path.IsPathRooted(includeFile)
                ? includeFile
                : Path.Combine(baseFolder, includeFile);

            await _nugetPackagesVersionUpdater.UpdateCentralPackageVersionsAsync(
                resolvedPath,
                latestVersionFromMyGet,
                excludedPackages);
        }
    }
    
    private async Task SwitchNpmPackageVersionsOfProjectsToPreRc(List<string> projects)
    {
        foreach (var project in projects)
        {
            var folder = Path.GetDirectoryName(project);

            await _npmPackagesUpdater.Update(
                folder,
                includePreRc: true);
        }
    }

    private async Task SwitchNpmPackageVersionsOfSolutionsToPreRc(List<string> solutionPaths)
    {
        foreach (var solutionPath in solutionPaths)
        {
            var solutionFolder = Path.GetDirectoryName(solutionPath);
            var solutionAngularFolder = GetSolutionAngularFolder(solutionFolder);

            await _npmPackagesUpdater.Update(
                solutionFolder,
                includePreRc: true);

            if (solutionAngularFolder != null)
            {
                await _npmPackagesUpdater.Update(
                    solutionAngularFolder,
                    includePreRc: true);
            }
        }
    }

    private List<string> GetSolutionPaths(CommandLineArgs commandLineArgs)
    {
        return Directory.GetFiles(GetDirectory(commandLineArgs), "*.sln", SearchOption.AllDirectories)
            .Concat(Directory.GetFiles(GetDirectory(commandLineArgs), "*.slnx", SearchOption.AllDirectories)).ToList();
    }

    private List<string> GetProjectPaths(CommandLineArgs commandLineArgs)
    {
        return Directory.GetFiles(GetDirectory(commandLineArgs), "*.csproj", SearchOption.AllDirectories).ToList();
    }

    private string GetDirectory(CommandLineArgs commandLineArgs)
    {
        return commandLineArgs.Options.GetOrNull(Options.SolutionDirectory.Short, Options.SolutionDirectory.Long)
               ?? commandLineArgs.Options.GetOrNull(Options.Directory.Short, Options.Directory.Long)
               ?? Directory.GetCurrentDirectory();
    }

    private List<string> GetCommaSeparatedOption(CommandLineArgs commandLineArgs, string shortName, string longName)
    {
        var raw = commandLineArgs.Options.GetOrNull(shortName, longName);
        return raw.IsNullOrWhiteSpace()
            ? new List<string>()
            : raw.Split(',').Select(s => s.Trim()).Where(s => !s.IsNullOrWhiteSpace()).ToList();
    }

    private string GetSolutionAngularFolder(string solutionFolder)
    {
        var upperAngularPath = Path.Combine(Directory.GetParent(solutionFolder)?.FullName ?? "", "angular");
        if (Directory.Exists(upperAngularPath))
        {
            return upperAngularPath;
        }

        var innerAngularPath = Path.Combine(solutionFolder, "angular");
        if (Directory.Exists(innerAngularPath))
        {
            return innerAngularPath;
        }

        return null;
    }    
    
    [CanBeNull]
    private string FindSolutionFolder(string projectFile)
    {
        var targetFolder = Path.GetDirectoryName(projectFile);

        do
        {
            if (Directory.GetParent(targetFolder) != null)
            {
                targetFolder = Directory.GetParent(targetFolder).FullName;
            }
            else
            {
                return Path.GetDirectoryName(projectFile);
            }

            if (Directory.GetFiles(targetFolder, "*.sln", SearchOption.TopDirectoryOnly)
                .Concat(Directory.GetFiles(targetFolder, "*.slnx", SearchOption.TopDirectoryOnly)).Any())
            {
                break;
            }
        } while (targetFolder != null);

        return targetFolder;
    }

    public static class Options
    {
        public static class SolutionDirectory
        {
            public const string Short = "sd";
            public const string Long = "solution-directory";
        }
        public static class Directory
        {
            public const string Short = "d";
            public const string Long = "directory";
        }
        public static class Include
        {
            public const string Short = "i";
            public const string Long = "include";
        }
        public static class Exclude
        {
            public const string Short = "ep";
            public const string Long = "exclude-packages";
        }
    }
}
