using System.IO;
using System.Threading.Tasks;
using Ionic.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ProjectBuilding;
using Volo.Abp.ProjectBuilding.Building;

namespace Volo.Abp.Cli.Commands
{
    public class NewCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<NewCommand> Logger { get; set; }

        protected ProjectBuilder ProjectBuilder { get; }

        public NewCommand(ProjectBuilder projectBuilder)
        {
            ProjectBuilder = projectBuilder;

            Logger = NullLogger<NewCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target == null)
            {
                Logger.LogWarning("Project name is missing.");
                Logger.LogWarning("");
                Logger.LogWarning("Usage:");
                Logger.LogWarning("  abp new <project-name> [-t|--template] [-d|--database-provider] [-o|--output-folder]");
                Logger.LogWarning("");
                Logger.LogWarning("Options:");
                Logger.LogWarning("-t|--template <template-name>");
                Logger.LogWarning("-d|--database-provider <database-provider>");
                Logger.LogWarning("-o|--output-folder <output-folder>");
                Logger.LogWarning("");
                Logger.LogWarning("Examples:");
                Logger.LogWarning("  abp new Acme.BookStore");
                Logger.LogWarning("  abp new Acme.BookStore -t mvc-module");
                Logger.LogWarning("  abp new Acme.BookStore -t mvc -d mongodb");
                Logger.LogWarning("  abp new Acme.BookStore -t mvc -d mongodb -o d:\\project");
                return;
            }

            Logger.LogInformation("Creating a new project...");
            Logger.LogInformation("Project name: " + commandLineArgs.Target);

            var result = await ProjectBuilder.BuildAsync(
                new ProjectBuildArgs(
                    SolutionName.Parse(commandLineArgs.Target),
                    GetDatabaseProviderOrNull(commandLineArgs),
                    commandLineArgs.Options.GetOrNull(Options.Template.Short, Options.Template.Long)
                )
            );

            var outputFolder = commandLineArgs.Options.GetOrNull(Options.OutputFolder.Short, Options.OutputFolder.Long);
            if (outputFolder != null)
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                outputFolder = Path.GetFullPath(outputFolder);
            }
            else
            {
                outputFolder = Directory.GetCurrentDirectory();
            }

            using (var templateFileStream = new MemoryStream(result.ZipContent))
            {
                using (var templateZipFile = ZipFile.Read(templateFileStream))
                {
                    templateZipFile.ExtractAll(outputFolder, ExtractExistingFileAction.Throw);
                }
            }

            Logger.LogInformation($"Successfully created the project '{commandLineArgs.Target}'");
            Logger.LogInformation($"The output folder is: '{outputFolder}'");
        }

        protected virtual DatabaseProvider GetDatabaseProviderOrNull(CommandLineArgs commandLineArgs)
        {
            var optionValue = commandLineArgs.Options.GetOrNull(Options.DatabaseProvider.Short, Options.DatabaseProvider.Long);
            switch (optionValue)
            {
                case "ef":
                    return DatabaseProvider.EntityFrameworkCore;
                case "mongodb":
                    return DatabaseProvider.MongoDb;
                default:
                    return DatabaseProvider.NotSpecified;
            }
        }

        public static class Options
        {
            public static class Template
            {
                public const string Short = "t";
                public const string Long = "template";
            }

            public static class DatabaseProvider
            {
                public const string Short = "d";
                public const string Long = "database-provider";
            }

            public static class OutputFolder
            {
                public const string Short = "o";
                public const string Long = "output-folder";
            }
        }
    }
}