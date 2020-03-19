using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification
{
    public class PackageSourceSwitcher : ITransientDependency
    {
        private readonly PackageSourceAdder _packageSourceAdder;
        private readonly NpmPackagesUpdater _npmPackagesUpdater;
        private readonly VoloNugetPackagesVersionUpdater _nugetPackagesVersionUpdater;

        public ILogger<PackageSourceSwitcher> Logger { get; set; }

        public PackageSourceSwitcher(PackageSourceAdder packageSourceAdder,
            NpmPackagesUpdater npmPackagesUpdater,
            VoloNugetPackagesVersionUpdater nugetPackagesVersionUpdater)
        {
            _packageSourceAdder = packageSourceAdder;
            _npmPackagesUpdater = npmPackagesUpdater;
            _nugetPackagesVersionUpdater = nugetPackagesVersionUpdater;
            Logger = NullLogger<PackageSourceSwitcher>.Instance;
        }

        public async Task SwitchToPreview(CommandLineArgs commandLineArgs)
        {
            _packageSourceAdder.Add("ABP Nightly", "https://www.myget.org/F/abp-nightly/api/v3/index.json");

            var solutionPath = GetSolutionPath(commandLineArgs);
            var solutionFolder = GetSolutionFolder(commandLineArgs);

            await _nugetPackagesVersionUpdater.UpdateSolutionAsync(
                solutionPath,
                true);

            await _npmPackagesUpdater.Update(
                solutionFolder,
                true);
        }

        public async Task SwitchToStable(CommandLineArgs commandLineArgs)
        {
            var solutionPath = GetSolutionPath(commandLineArgs);
            var solutionFolder = GetSolutionFolder(commandLineArgs);

            await _nugetPackagesVersionUpdater.UpdateSolutionAsync(
                solutionPath,
                false,
                true);

            await _npmPackagesUpdater.Update(
                solutionFolder,
                false,
                true);
        }

        private string GetSolutionPath(CommandLineArgs commandLineArgs)
        {
            var directory = commandLineArgs.Options.GetOrNull(Options.SolutionDirectory.Short, Options.SolutionDirectory.Long)
                            ?? Directory.GetCurrentDirectory();

            var solutionPath = Directory.GetFiles(directory, "*.sln").FirstOrDefault();

            if (solutionPath == null)
            {
                var subDirectories = Directory.GetDirectories(directory);

                foreach (var subDirectory in subDirectories)
                {
                    var slnInSubDirectory = Directory.GetFiles(subDirectory, "*.sln").FirstOrDefault();

                    if (slnInSubDirectory != null)
                    {
                        return Path.Combine(subDirectory, slnInSubDirectory);
                    }
                }

                Logger.LogError("There is no solution or more that one solution in current directory.");
                return null;
            }

            return solutionPath;
        }

        private string GetSolutionFolder(CommandLineArgs commandLineArgs)
        {
            return commandLineArgs.Options.GetOrNull(Options.SolutionDirectory.Short, Options.SolutionDirectory.Long)
                   ?? Directory.GetCurrentDirectory();
        }

        public static class Options
        {
            public static class SolutionDirectory
            {
                public const string Short = "sd";
                public const string Long = "solution-directory";
            }
        }
    }
}
