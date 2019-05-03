using System;
using System.IO;
using System.Threading.Tasks;
using Ionic.Zip;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ProjectBuilding;
using Volo.Abp.SolutionTemplating;
using Volo.Abp.SolutionTemplating.Building;
using Volo.Abp.SolutionTemplating.Zipping;

namespace Volo.Abp.Cli.Commands
{
    public class NewProjectCommand : IConsoleCommand, ITransientDependency
    {
        protected ProjectBuilder ProjectBuilder { get; }

        public NewProjectCommand(ProjectBuilder projectBuilder)
        {
            ProjectBuilder = projectBuilder;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target == null)
            {
                Console.WriteLine("Project name is missing.");
                Console.WriteLine("Usage:");
                Console.WriteLine("  abp new <project-name> [-t <template-name>]");
                Console.WriteLine("Examples:");
                Console.WriteLine("  abp new Acme.BookStore");
                Console.WriteLine("  abp new Acme.BookStore mvc");
                return;
            }

            Console.WriteLine("Creating a new solution");
            Console.WriteLine("Solution name: " + commandLineArgs.Target);

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
                public const string Long = "databaseProvider";
            }
        }
    }
}