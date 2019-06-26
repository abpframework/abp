using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;

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
                throw new CliUsageException("Project name is missing!" + Environment.NewLine + Environment.NewLine + await GetUsageInfo());
            }

            Logger.LogInformation("Creating a new project...");
            Logger.LogInformation("Project name: " + commandLineArgs.Target);

            var result = await ProjectBuilder.BuildAsync(
                new ProjectBuildArgs(
                    SolutionName.Parse(commandLineArgs.Target),
                    commandLineArgs.Options.GetOrNull(Options.Template.Short, Options.Template.Long),
                    GetDatabaseProviderOrNull(commandLineArgs),
                    commandLineArgs.Options
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
                var zipInputStream = new ZipInputStream(templateFileStream);
                var zipEntry = zipInputStream.GetNextEntry();
                while (zipEntry != null)
                {
                    var fullZipToPath = Path.Combine(outputFolder, zipEntry.Name);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    var fileName = Path.GetFileName(fullZipToPath);
                    if (fileName.Length == 0)
                    {
                        zipEntry = zipInputStream.GetNextEntry();
                        continue;
                    }

                    var buffer = new byte[4096]; // 4K is optimum
                    using (var streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipInputStream, streamWriter, buffer);
                    }
                    zipEntry = zipInputStream.GetNextEntry();
                }
            }

            Logger.LogInformation($"Successfully created the project '{commandLineArgs.Target}'");
            Logger.LogInformation($"The output folder is: '{outputFolder}'");
        }

        public Task<string> GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("  abp new <project-name> [-t|--template] [-d|--database-provider] [-o|--output-folder]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("-t|--template <template-name>");
            sb.AppendLine("-o|--output-folder <output-folder>");
            sb.AppendLine("-d|--database-provider <database-provider>  (if supported by the template)");
            sb.AppendLine("--tiered                                    (if supported by the template)");
            sb.AppendLine("--no-ui                                     (if supported by the template)");
            sb.AppendLine("");
            sb.AppendLine("Some examples:");
            sb.AppendLine("  abp new Acme.BookStore");
            sb.AppendLine("  abp new Acme.BookStore --tiered");
            sb.AppendLine("  abp new Acme.BookStore -t mvc-module");
            sb.AppendLine("  abp new Acme.BookStore -t mvc-module no-ui");
            sb.AppendLine("  abp new Acme.BookStore -d mongodb");
            sb.AppendLine("  abp new Acme.BookStore -t mvc -d mongodb");
            sb.AppendLine("  abp new Acme.BookStore -t mvc -d mongodb -o d:\\project");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info.");

            return Task.FromResult(sb.ToString());
        }

        public Task<string> GetShortDescriptionAsync()
        {
            return Task.FromResult("Generates a new solution based on the ABP startup templates.");
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