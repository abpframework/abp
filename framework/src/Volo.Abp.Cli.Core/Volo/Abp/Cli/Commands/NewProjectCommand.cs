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
    public class NewProjectCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<NewProjectCommand> Logger { get; set; }

        protected ProjectBuilder ProjectBuilder { get; }

        public NewProjectCommand(ProjectBuilder projectBuilder)
        {
            ProjectBuilder = projectBuilder;

            Logger = NullLogger<NewProjectCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target == null)
            {
                Logger.LogWarning("Project name is missing.");
                Logger.LogWarning("Usage:");
                Logger.LogWarning("  abp new <project-name> [-t|--template] [-d|--database-provider]");
                Logger.LogWarning("");
                Logger.LogWarning("");
                Logger.LogWarning("");
                Logger.LogWarning("Examples:");
                Logger.LogWarning("  abp new Acme.BookStore");
                Logger.LogWarning("  abp new Acme.BookStore -t mvc-module");
                Logger.LogWarning("  abp new Acme.BookStore -t mvc -d mongodb");
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

            using (var templateFileStream = new MemoryStream(result.ZipContent))
            {
                using (var templateZipFile = ZipFile.Read(templateFileStream))
                {
                    templateZipFile.ExtractAll(Directory.GetCurrentDirectory(), ExtractExistingFileAction.Throw);
                }
            }

            Logger.LogInformation($"Successfully created the project '{commandLineArgs.Target}'");
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
        }
    }
}