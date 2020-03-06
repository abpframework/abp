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

            await _nugetPackagesVersionUpdater.UpdateSolutionAsync(
                GetSolutionPath(commandLineArgs),
                true);

            await _npmPackagesUpdater.Update(
                Path.GetFileName(GetSolutionPath(commandLineArgs)),
                true);
        }

        public async Task SwitchToStable(CommandLineArgs commandLineArgs)
        {
            await _nugetPackagesVersionUpdater.UpdateSolutionAsync(
                GetSolutionPath(commandLineArgs),
                false,
                true);

            await _npmPackagesUpdater.Update(
                Path.GetFileName(GetSolutionPath(commandLineArgs)),
                false, 
                true);
        }

      
        private string GetSolutionPath(CommandLineArgs commandLineArgs)
        {
            var solutionPath = commandLineArgs.Options.GetOrNull(Options.SolutionPath.Short, Options.SolutionPath.Long);

            if (solutionPath == null)
            {
                try
                {
                    solutionPath = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln").Single();
                }
                catch (Exception)
                {
                    Logger.LogError("There is no solution or more that one solution in current directory.");
                    throw;
                }
            }

            return solutionPath;
        }

        public static class Options
        {
            public static class SolutionPath
            {
                public const string Short = "sp";
                public const string Long = "solution-path";
            }
        }
    }
}
