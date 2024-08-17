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
            await SwitchSolutionsToNightlyPreview(solutionPaths);
        }
        else
        {
            var projectPaths = GetProjectPaths(commandLineArgs);
            
            await SwitchProjectsToNightlyPreview(projectPaths);
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

    private async Task SwitchProjectsToNightlyPreview(List<string> projects)
    {
        foreach (var project in projects)
        {
            var folder = Path.GetDirectoryName(project);

            _packageSourceManager.Add(FindSolutionFolder(project) ?? folder, "ABP Nightly",
                "https://www.myget.org/F/abp-nightly/api/v3/index.json", "Volo.*");

            await _nugetPackagesVersionUpdater.UpdateSolutionAsync(
                project,
                true);

            await _npmPackagesUpdater.Update(
                folder,
                true);
        }
    }

    private async Task SwitchSolutionsToNightlyPreview(List<string> solutionPaths)
    {
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
        return Directory.GetFiles(GetDirectory(commandLineArgs), "*.sln", SearchOption.AllDirectories).ToList();
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

            if (Directory.GetFiles(targetFolder, "*.sln", SearchOption.TopDirectoryOnly).Any())
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
    }
}
