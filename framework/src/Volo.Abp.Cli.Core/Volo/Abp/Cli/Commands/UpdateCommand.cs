using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class UpdateCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<UpdateCommand> Logger { get; set; }

        private readonly VoloNugetPackagesVersionUpdater _nugetPackagesVersionUpdater;
        private readonly NpmPackagesUpdater _npmPackagesUpdater;

        public UpdateCommand(VoloNugetPackagesVersionUpdater nugetPackagesVersionUpdater,
            NpmPackagesUpdater npmPackagesUpdater)
        {
            _nugetPackagesVersionUpdater = nugetPackagesVersionUpdater;
            _npmPackagesUpdater = npmPackagesUpdater;

            Logger = NullLogger<UpdateCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var updateNpm = commandLineArgs.Options.ContainsKey(Options.Packages.Npm);
            var updateNuget = commandLineArgs.Options.ContainsKey(Options.Packages.NuGet);

            var directory = commandLineArgs.Options.GetOrNull(Options.SolutionPath.Short, Options.SolutionPath.Long) ??
                            Directory.GetCurrentDirectory();

            if (updateNuget || !updateNpm)
            {
                await UpdateNugetPackages(commandLineArgs, directory);
            }

            if (updateNpm || !updateNuget)
            {
                UpdateNpmPackages(directory);
            }
        }

        private void UpdateNpmPackages(string directory)
        {
            _npmPackagesUpdater.Update(directory);
        }

        private async Task UpdateNugetPackages(CommandLineArgs commandLineArgs, string directory)
        {
            var includePreviews =
                commandLineArgs.Options.GetOrNull(Options.IncludePreviews.Short, Options.IncludePreviews.Long) != null;

            var solution = Directory.GetFiles(directory, "*.sln").FirstOrDefault();

            if (solution != null)
            {
                var solutionName = Path.GetFileName(solution).RemovePostFix(".sln");

                await _nugetPackagesVersionUpdater.UpdateSolutionAsync(solution, includePreviews);

                Logger.LogInformation($"Volo packages are updated in {solutionName} solution.");
                return;
            }

            var project = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj").FirstOrDefault();

            if (project != null)
            {
                var projectName = Path.GetFileName(project).RemovePostFix(".csproj");

                await _nugetPackagesVersionUpdater.UpdateProjectAsync(project, includePreviews);

                Logger.LogInformation($"Volo packages are updated in {projectName} project.");
                return;
            }

            throw new CliUsageException(
                "No solution or project found in this directory." +
                Environment.NewLine + Environment.NewLine +
                GetUsageInfo()
            );
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  abp update  [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("-p|--include-previews                       (if supported by the template)");
            sb.AppendLine("--npm                                       (Only updates NPM packages)");
            sb.AppendLine("--nuget                                     (Only updates Nuget packages)");
            sb.AppendLine("");
            sb.AppendLine("Some examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp update");
            sb.AppendLine("  abp update -p");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Update all ABP related NuGet packages and NPM packages in a solution or project to the latest version.";
        }

        public static class Options
        {
            public static class SolutionPath
            {
                public const string Short = "sp";
                public const string Long = "solution-path";
            }
            
            public static class IncludePreviews
            {
                public const string Short = "p";
                public const string Long = "include-previews";
            }

            public static class Packages
            {
                public const string Npm = "npm";
                public const string NuGet = "nuget";
            }
        }
    }
}
