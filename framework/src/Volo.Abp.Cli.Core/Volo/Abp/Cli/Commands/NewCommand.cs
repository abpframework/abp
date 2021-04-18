using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Auth;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.Console;
using Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.Commands
{
    public class NewCommand : IConsoleCommand, ITransientDependency
    {
        private readonly EfCoreMigrationManager _efCoreMigrationManager;
        public ILogger<NewCommand> Logger { get; set; }

        protected TemplateProjectBuilder TemplateProjectBuilder { get; }
        public ITemplateInfoProvider TemplateInfoProvider { get; }
        public ConnectionStringProvider ConnectionStringProvider { get; }

        public NewCommand(TemplateProjectBuilder templateProjectBuilder
            , ITemplateInfoProvider templateInfoProvider,
            EfCoreMigrationManager efCoreMigrationManager,
            ConnectionStringProvider connectionStringProvider)
        {
            _efCoreMigrationManager = efCoreMigrationManager;
            TemplateProjectBuilder = templateProjectBuilder;
            TemplateInfoProvider = templateInfoProvider;
            ConnectionStringProvider = connectionStringProvider;

            Logger = NullLogger<NewCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var projectName = NamespaceHelper.NormalizeNamespace(commandLineArgs.Target);

            if (projectName == null)
            {
                throw new CliUsageException(
                    "Project name is missing!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }

            Logger.LogInformation("Creating your project...");
            Logger.LogInformation("Project name: " + projectName);

            var template = commandLineArgs.Options.GetOrNull(Options.Template.Short, Options.Template.Long);
            if (template != null)
            {
                Logger.LogInformation("Template: " + template);
            }
            else
            {
                template = (await TemplateInfoProvider.GetDefaultAsync()).Name;
            }

            var version = commandLineArgs.Options.GetOrNull(Options.Version.Short, Options.Version.Long);
            if (version != null)
            {
                Logger.LogInformation("Version: " + version);
            }

            var isTiered = commandLineArgs.Options.ContainsKey(Options.Tiered.Long);
            if (isTiered)
            {
                Logger.LogInformation("Tiered: yes");
            }

            var preview = commandLineArgs.Options.ContainsKey(Options.Preview.Long);
            if (preview)
            {
                Logger.LogInformation("Preview: yes");
            }

            var databaseProvider = GetDatabaseProvider(commandLineArgs);
            if (databaseProvider != DatabaseProvider.NotSpecified)
            {
                Logger.LogInformation("Database provider: " + databaseProvider);
            }

            var connectionString = GetConnectionString(commandLineArgs);
            if (connectionString != null)
            {
                Logger.LogInformation("Connection string: " + connectionString);
            }

            var databaseManagementSystem = GetDatabaseManagementSystem(commandLineArgs);
            if (databaseManagementSystem != DatabaseManagementSystem.NotSpecified)
            {
                Logger.LogInformation("DBMS: " + databaseManagementSystem);
            }

            var uiFramework = GetUiFramework(commandLineArgs);
            if (uiFramework != UiFramework.NotSpecified)
            {
                Logger.LogInformation("UI Framework: " + uiFramework);
            }

            var publicWebSite = uiFramework != UiFramework.None && commandLineArgs.Options.ContainsKey(Options.PublicWebSite.Long);
            if (publicWebSite)
            {
                Logger.LogInformation("Public Web Site: yes");
            }

            var mobileApp = GetMobilePreference(commandLineArgs);
            if (mobileApp != MobileApp.None)
            {
                Logger.LogInformation("Mobile App: " + mobileApp);
            }

            var gitHubAbpLocalRepositoryPath = commandLineArgs.Options.GetOrNull(Options.GitHubAbpLocalRepositoryPath.Long);
            if (gitHubAbpLocalRepositoryPath != null)
            {
                Logger.LogInformation("GitHub Abp Local Repository Path: " + gitHubAbpLocalRepositoryPath);
            }

            var gitHubVoloLocalRepositoryPath = commandLineArgs.Options.GetOrNull(Options.GitHubVoloLocalRepositoryPath.Long);
            if (gitHubVoloLocalRepositoryPath != null)
            {
                Logger.LogInformation("GitHub Volo Local Repository Path: " + gitHubVoloLocalRepositoryPath);
            }

            var templateSource = commandLineArgs.Options.GetOrNull(Options.TemplateSource.Short, Options.TemplateSource.Long);
            if (templateSource != null)
            {
                Logger.LogInformation("Template Source: " + templateSource);
            }

            var createSolutionFolder = GetCreateSolutionFolderPreference(commandLineArgs);

            var outputFolder = commandLineArgs.Options.GetOrNull(Options.OutputFolder.Short, Options.OutputFolder.Long);

            var outputFolderRoot =
                outputFolder != null ? Path.GetFullPath(outputFolder) : Directory.GetCurrentDirectory();

            SolutionName solutionName;
            if (MicroserviceServiceTemplateBase.IsMicroserviceServiceTemplate(template))
            {
                var microserviceSolutionName = FindMicroserviceSolutionName(outputFolderRoot);

                if (microserviceSolutionName == null)
                {
                    throw new CliUsageException("This command should be run inside a folder that contains a microservice solution!");
                }

                solutionName = SolutionName.Parse(microserviceSolutionName, projectName);
                outputFolder = MicroserviceServiceTemplateBase.CalculateTargetFolder(outputFolderRoot, projectName);
                uiFramework = uiFramework == UiFramework.NotSpecified ? FindMicroserviceSolutionUiFramework(outputFolderRoot) : uiFramework;
            }
            else
            {
                solutionName = SolutionName.Parse(projectName);

                outputFolder = createSolutionFolder ?
                    Path.Combine(outputFolderRoot, SolutionName.Parse(projectName).FullName) :
                    outputFolderRoot;
            }

            Volo.Abp.IO.DirectoryHelper.CreateIfNotExists(outputFolder);

            Logger.LogInformation("Output folder: " + outputFolder);

            if (connectionString == null &&
                databaseManagementSystem != DatabaseManagementSystem.NotSpecified &&
                databaseManagementSystem != DatabaseManagementSystem.SQLServer)
            {
                connectionString = ConnectionStringProvider.GetByDbms(databaseManagementSystem, outputFolder);
            }

            commandLineArgs.Options.Add(CliConsts.Command, commandLineArgs.Command);

            var result = await TemplateProjectBuilder.BuildAsync(
                new ProjectBuildArgs(
                    solutionName,
                    template,
                    version,
                    databaseProvider,
                    databaseManagementSystem,
                    uiFramework,
                    mobileApp,
                    publicWebSite,
                    gitHubAbpLocalRepositoryPath,
                    gitHubVoloLocalRepositoryPath,
                    templateSource,
                    commandLineArgs.Options,
                    connectionString
                )
            );

            using (var templateFileStream = new MemoryStream(result.ZipContent))
            {
                using (var zipInputStream = new ZipInputStream(templateFileStream))
                {
                    var zipEntry = zipInputStream.GetNextEntry();
                    while (zipEntry != null)
                    {
                        if (string.IsNullOrWhiteSpace(zipEntry.Name))
                        {
                            zipEntry = zipInputStream.GetNextEntry();
                            continue;
                        }

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
            }

            Logger.LogInformation($"'{projectName}' has been successfully created to '{outputFolder}'");


            if (AppTemplateBase.IsAppTemplate(template))
            {
                var isCommercial = template == AppProTemplate.TemplateName;
                OpenThanksPage(uiFramework, databaseProvider, isTiered || commandLineArgs.Options.ContainsKey("separate-identity-server"), isCommercial);
            }
            else if (MicroserviceTemplateBase.IsMicroserviceTemplate(template))
            {
                OpenMicroserviceDocumentPage();
            }
        }

        private string FindMicroserviceSolutionName(string outputFolderRoot)
        {
            var slnFile = Directory.GetFiles(outputFolderRoot, "*.sln").FirstOrDefault();

            if (slnFile == null)
            {
                return null;
            }

            return Path.GetFileName(slnFile).RemovePostFix(".sln");
        }

        private UiFramework FindMicroserviceSolutionUiFramework(string outputFolderRoot)
        {
            if (Directory.Exists(Path.Combine(outputFolderRoot, "apps", "blazor")))
            {
                return UiFramework.Blazor;
            }
            if (Directory.Exists(Path.Combine(outputFolderRoot, "apps", "web")))
            {
                return UiFramework.Mvc;
            }
            if (Directory.Exists(Path.Combine(outputFolderRoot, "apps", "angular")))
            {
                return UiFramework.Angular;
            }

            return UiFramework.None;
        }

        private void OpenThanksPage(UiFramework uiFramework, DatabaseProvider databaseProvider, bool tiered, bool commercial)
        {
            uiFramework = uiFramework == UiFramework.NotSpecified || uiFramework == UiFramework.None ? UiFramework.Mvc : uiFramework;
            databaseProvider = databaseProvider == DatabaseProvider.NotSpecified ? DatabaseProvider.EntityFrameworkCore : databaseProvider;

            var urlPrefix = commercial ? "commercial" : "www";
            var tieredYesNo = tiered ? "yes" : "no";
            var url = $"https://{urlPrefix}.abp.io/project-created-success?ui={uiFramework:g}&db={databaseProvider:g}&tiered={tieredYesNo}";

            CmdHelper.OpenWebPage(url);
        }

        private void OpenMicroserviceDocumentPage()
        {
            var url = "https://docs.abp.io/en/commercial/latest/startup-templates/microservice/index";

            CmdHelper.OpenWebPage(url);
        }

        private bool GetCreateSolutionFolderPreference(CommandLineArgs commandLineArgs)
        {
            var longKey = commandLineArgs.Options.ContainsKey(Options.CreateSolutionFolder.Long);

            if (longKey == false)
            {
                return commandLineArgs.Options.ContainsKey(Options.CreateSolutionFolder.Short);
            }

            return longKey;
        }

        private static string GetConnectionString(CommandLineArgs commandLineArgs)
        {
            var connectionString = commandLineArgs.Options.GetOrNull(Options.ConnectionString.Short, Options.ConnectionString.Long);
            return string.IsNullOrWhiteSpace(connectionString) ? null : connectionString;
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  abp new <project-name> [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("");
            sb.AppendLine("-t|--template <template-name>               (default: app)");
            sb.AppendLine("-u|--ui <ui-framework>                      (if supported by the template)");
            sb.AppendLine("-m|--mobile <mobile-framework>              (if supported by the template)");
            sb.AppendLine("-d|--database-provider <database-provider>  (if supported by the template)");
            sb.AppendLine("-o|--output-folder <output-folder>          (default: current folder)");
            sb.AppendLine("-v|--version <version>                      (default: latest version)");
            sb.AppendLine("--preview                                   (Use latest pre-release version if there is at least one pre-release after latest stable version)");
            sb.AppendLine("-ts|--template-source <template-source>     (your local or network abp template source)");
            sb.AppendLine("-csf|--create-solution-folder               (default: true)");
            sb.AppendLine("-cs|--connection-string <connection-string> (your database connection string)");
            sb.AppendLine("--dbms <database-management-system>         (your database management system)");
            sb.AppendLine("--tiered                                    (if supported by the template)");
            sb.AppendLine("--no-ui                                     (if supported by the template)");
            sb.AppendLine("--no-random-port                            (Use template's default ports)");
            sb.AppendLine("--separate-identity-server                  (if supported by the template)");
            sb.AppendLine("--local-framework-ref --abp-path <your-local-abp-repo-path>  (keeps local references to projects instead of replacing with NuGet package references)");
            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp new Acme.BookStore");
            sb.AppendLine("  abp new Acme.BookStore --tiered");
            sb.AppendLine("  abp new Acme.BookStore -u angular");
            sb.AppendLine("  abp new Acme.BookStore -u angular -d mongodb");
            sb.AppendLine("  abp new Acme.BookStore -m none");
            sb.AppendLine("  abp new Acme.BookStore -m react-native");
            sb.AppendLine("  abp new Acme.BookStore -d mongodb");
            sb.AppendLine("  abp new Acme.BookStore -d mongodb -o d:\\my-project");
            sb.AppendLine("  abp new Acme.BookStore -t module");
            sb.AppendLine("  abp new Acme.BookStore -t module --no-ui");
            sb.AppendLine("  abp new Acme.BookStore -t console");
            sb.AppendLine("  abp new Acme.BookStore -ts \"D:\\localTemplate\\abp\"");
            sb.AppendLine("  abp new Acme.BookStore -csf false");
            sb.AppendLine("  abp new Acme.BookStore --local-framework-ref --abp-path \"D:\\github\\abp\"");
            sb.AppendLine("  abp new Acme.BookStore --dbms mysql");
            sb.AppendLine("  abp new Acme.BookStore --connection-string \"Server=myServerName\\myInstanceName;Database=myDatabase;User Id=myUsername;Password=myPassword\"");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Generate a new solution based on the ABP startup templates.";
        }

        protected virtual DatabaseProvider GetDatabaseProvider(CommandLineArgs commandLineArgs)
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

        protected virtual DatabaseManagementSystem GetDatabaseManagementSystem(CommandLineArgs commandLineArgs)
        {
            var optionValue = commandLineArgs.Options.GetOrNull(Options.DatabaseManagementSystem.Short, Options.DatabaseManagementSystem.Long);

            if (optionValue == null)
            {
                return DatabaseManagementSystem.NotSpecified;
            }

            switch (optionValue.ToLowerInvariant())
            {
                case "sqlserver":
                    return DatabaseManagementSystem.SQLServer;
                case "mysql":
                    return DatabaseManagementSystem.MySQL;
                case "postgresql":
                    return DatabaseManagementSystem.PostgreSQL;
                case "oracle-devart":
                    return DatabaseManagementSystem.OracleDevart;
                case "sqlite":
                    return DatabaseManagementSystem.SQLite;
                case "oracle":
                    return DatabaseManagementSystem.Oracle;
                default:
                    return DatabaseManagementSystem.NotSpecified;
            }
        }

        protected virtual UiFramework GetUiFramework(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Options.ContainsKey("no-ui"))
            {
                return UiFramework.None;
            }

            var optionValue = commandLineArgs.Options.GetOrNull(Options.UiFramework.Short, Options.UiFramework.Long);
            switch (optionValue)
            {
                case "none":
                    return UiFramework.None;
                case "mvc":
                    return UiFramework.Mvc;
                case "angular":
                    return UiFramework.Angular;
                case "blazor":
                    return UiFramework.Blazor;
                case "blazor-server":
                    return UiFramework.BlazorServer;
                default:
                    return UiFramework.NotSpecified;
            }
        }

        protected virtual MobileApp GetMobilePreference(CommandLineArgs commandLineArgs)
        {
            var optionValue = commandLineArgs.Options.GetOrNull(Options.Mobile.Short, Options.Mobile.Long);

            switch (optionValue)
            {
                case "none":
                    return MobileApp.None;
                case "react-native":
                    return MobileApp.ReactNative;
                default:
                    return MobileApp.None;
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

            public static class DatabaseManagementSystem
            {
                public const string Short = "dbms";
                public const string Long = "database-management-system";
            }

            public static class OutputFolder
            {
                public const string Short = "o";
                public const string Long = "output-folder";
            }

            public static class GitHubAbpLocalRepositoryPath
            {
                public const string Long = "abp-path";
            }

            public static class GitHubVoloLocalRepositoryPath
            {
                public const string Long = "volo-path";
            }

            public static class Version
            {
                public const string Short = "v";
                public const string Long = "version";
            }

            public static class UiFramework
            {
                public const string Short = "u";
                public const string Long = "ui";
            }

            public static class Mobile
            {
                public const string Short = "m";
                public const string Long = "mobile";
            }

            public static class PublicWebSite
            {
                public const string Long = "with-public-website";
            }

            public static class TemplateSource
            {
                public const string Short = "ts";
                public const string Long = "template-source";
            }

            public static class ConnectionString
            {
                public const string Short = "cs";
                public const string Long = "connection-string";
            }

            public static class CreateSolutionFolder
            {
                public const string Short = "csf";
                public const string Long = "create-solution-folder";
            }

            public static class Tiered
            {
                public const string Long = "tiered";
            }

            public static class Preview
            {
                public const string Long = "preview";
            }
        }
    }
}
