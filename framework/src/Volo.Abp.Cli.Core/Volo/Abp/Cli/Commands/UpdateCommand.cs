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

        public UpdateCommand(VoloNugetPackagesVersionUpdater nugetPackagesVersionUpdater, NpmPackagesUpdater npmPackagesUpdater)
        {
            _nugetPackagesVersionUpdater = nugetPackagesVersionUpdater;
            _npmPackagesUpdater = npmPackagesUpdater;

            Logger = NullLogger<UpdateCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            UpdateNugetPackages(commandLineArgs);
            UpdateNpmPackages();
            return;
        }

        private void UpdateNpmPackages()
        {
            _npmPackagesUpdater.Update(Directory.GetCurrentDirectory());
        }

        private void UpdateNugetPackages(CommandLineArgs commandLineArgs)
        {
            var includePreviews =
                commandLineArgs.Options.GetOrNull(Options.IncludePreviews.Short, Options.IncludePreviews.Long) != null;

            var solution = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln").FirstOrDefault();

            if (solution != null)
            {
                var solutionName = Path.GetFileName(solution).RemovePostFix(".sln");

                _nugetPackagesVersionUpdater.UpdateSolution(solution, includePreviews);

                Logger.LogInformation($"Volo packages are updated in {solutionName} solution.");
                return;
            }

            var project = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj").FirstOrDefault();

            if (project != null)
            {
                var projectName = Path.GetFileName(project).RemovePostFix(".csproj");

                _nugetPackagesVersionUpdater.UpdateProject(project, includePreviews);

                Logger.LogInformation($"Volo packages are updated in {projectName} project.");
                return;
            }

            throw new CliUsageException("No solution or project found in this directory." + Environment.NewLine +
                                        Environment.NewLine + GetUsageInfo());
        }

        public Task<string> GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("  abp update  [-p|--include-previews]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("--include-previews                          (if supported by the template)");
            sb.AppendLine("");
            sb.AppendLine("Some examples:");
            sb.AppendLine("  abp update");
            sb.AppendLine("  abp update --include-previews");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info.");

            return Task.FromResult(sb.ToString());
        }

        public Task<string> GetShortDescriptionAsync()
        {
            return Task.FromResult("Automatically updates all ABP related packages in a" +
                                   " solution or project to the latest versions.");
        }

        public static class Options
        {
            public static class IncludePreviews
            {
                public const string Short = "p";
                public const string Long = "include-previews";
            }
        }
    }
}
