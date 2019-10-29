using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class AddModuleCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<AddModuleCommand> Logger { get; set; }

        protected SolutionModuleAdder SolutionModuleAdder { get; }

        public AddModuleCommand(SolutionModuleAdder solutionModuleAdder)
        {
            SolutionModuleAdder = solutionModuleAdder;
            Logger = NullLogger<AddModuleCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target == null)
            {
                throw new CliUsageException(
                    "Module name is missing!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            var skipDbMigrations = Convert.ToBoolean(
                commandLineArgs.Options.GetOrNull(Options.DbMigrations.Skip) ?? "false");

            await SolutionModuleAdder.AddAsync(
                GetSolutionFile(commandLineArgs),
                commandLineArgs.Target,
                commandLineArgs.Options.GetOrNull(Options.StartupProject.Short, Options.StartupProject.Long),
                skipDbMigrations
            );
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("'add-module' command is used to add a multi-package ABP module to a solution.");
            sb.AppendLine("It should be used in a folder containing a .sln file.");
            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("  abp add-module <module-name> [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("  -s|--solution <solution-file>    Specify the solution file explicitly.");
            sb.AppendLine("  --skip-db-migrations <boolean>    Specify if a new migration will be added or not.");
            sb.AppendLine("  -sp|--startup-project <startup-project-path>    Relative path to the project folder of the startup project. Default value is the current folder.");
            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp add-module Volo.Blogging                      Adds the module to the current solution.");
            sb.AppendLine("  abp add-module Volo.Blogging -s Acme.BookStore    Adds the module to the given solution.");
            sb.AppendLine("  abp add-module Volo.Blogging -s Acme.BookStore --skip-db-migrations false    Adds the module to the given solution but doesn't create a database migration.");
            sb.AppendLine(@"  abp add-module Volo.Blogging -s Acme.BookStore -sp ..\Acme.BookStore.Web\Acme.BookStore.Web.csproj   Adds the module to the given solution and specify migration startup project.");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Adds a multi-package module to a solution by finding all packages of the module, " +
                   "finding related projects in the solution and adding each package to the" +
                   " corresponding project in the solution.";
        }

        protected virtual string GetSolutionFile(CommandLineArgs commandLineArgs)
        {
            var providedSolutionFile = PathHelper.NormalizePath(
                commandLineArgs.Options.GetOrNull(
                    Options.Solution.Short,
                    Options.Solution.Long
                )
            );

            if (!providedSolutionFile.IsNullOrWhiteSpace())
            {
                return providedSolutionFile;
            }

            var foundSolutionFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sln");
            if (foundSolutionFiles.Length == 1)
            {
                return foundSolutionFiles[0];
            }

            if (foundSolutionFiles.Length == 0)
            {
                throw new CliUsageException("'abp add-module' command should be used inside a folder containing a .sln file!");
            }

            //foundSolutionFiles.Length > 1

            var sb = new StringBuilder("There are multiple solution (.sln) files in the current directory. Please specify one of the files below:");

            foreach (var foundSolutionFile in foundSolutionFiles)
            {
                sb.AppendLine("* " + foundSolutionFile);
            }

            sb.AppendLine("Example:");
            sb.AppendLine($"abp add-module {commandLineArgs.Target} -p {foundSolutionFiles[0]}");

            throw new CliUsageException(sb.ToString());
        }

        public static class Options
        {
            public static class Solution
            {
                public const string Short = "s";
                public const string Long = "solution";
            }

            public static class DbMigrations
            {
                public const string Skip = "skip-db-migrations";
            }

            public static class StartupProject 
            {
                public const string Short = "sp";
                public const string Long = "startup-project";
            }
        }
    }
}
