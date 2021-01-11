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
            var version = commandLineArgs.Options.GetOrNull(Options.Version.Short, Options.Version.Long);

            if (updateNuget || !updateNpm)
            {
                await UpdateNugetPackages(commandLineArgs, directory, version);
            }

            if (updateNpm || !updateNuget)
            {
                await UpdateNpmPackages(directory, version);
            }
        }

        private async Task UpdateNpmPackages(string directory, string version)
        {
            await _npmPackagesUpdater.Update(directory, version: version);
        }

        private async Task UpdateNugetPackages(CommandLineArgs commandLineArgs, string directory, string version)
        {

            var solution = commandLineArgs.Options.GetOrNull(Options.SolutionName.Short, Options.SolutionName.Long);
            if (solution.IsNullOrWhiteSpace())
            {
                solution = Directory.GetFiles(directory, "*.sln", SearchOption.AllDirectories).FirstOrDefault();
            }

            var checkAll = commandLineArgs.Options.ContainsKey(Options.CheckAll.Long);

            if (solution != null)
            {
                var solutionName = Path.GetFileName(solution).RemovePostFix(".sln");

                await _nugetPackagesVersionUpdater.UpdateSolutionAsync(solution, checkAll: checkAll, version: version);

                Logger.LogInformation($"Volo packages are updated in {solutionName} solution.");
                return;
            }

            var project = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj").FirstOrDefault();

            if (project != null)
            {
                var projectName = Path.GetFileName(project).RemovePostFix(".csproj");

                await _nugetPackagesVersionUpdater.UpdateProjectAsync(project, checkAll: checkAll, version: version);

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
            sb.AppendLine("  abp update [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("-p|--include-previews                       (if supported by the template)");
            sb.AppendLine("--npm                                       (Only updates NPM packages)");
            sb.AppendLine("--nuget                                     (Only updates Nuget packages)");
            sb.AppendLine("-sp|--solution-path                         (Specify the solution path)");
            sb.AppendLine("-sn|--solution-name                         (Specify the solution name)");
            sb.AppendLine("--check-all                                 (Check the new version of each package separately)");
            sb.AppendLine("-v|--version <version>                      (default: latest version)");
            sb.AppendLine("");
            sb.AppendLine("Some examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp update");
            sb.AppendLine("  abp update -p");
            sb.AppendLine("  abp update -sp \"D:\\projects\\\" -sn Acme.BookStore");
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

            public static class SolutionName
            {
                public const string Short = "sn";
                public const string Long = "solution-name";
            }

            public static class Packages
            {
                public const string Npm = "npm";
                public const string NuGet = "nuget";
            }

            public static class CheckAll
            {
                public const string Long = "check-all";
            }

            public static class Version
            {
                public const string Short = "v";
                public const string Long = "version";
            }
        }
    }
}
