using System;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;
using Volo.Abp.Cli.Utils;

namespace Volo.Abp.Cli.Commands
{
    public abstract class ProjectCreationCommandBase
    {
        public ConnectionStringProvider ConnectionStringProvider { get; }
        public ICmdHelper CmdHelper { get; }
        public ILogger<NewCommand> Logger { get; set; }

        public ProjectCreationCommandBase(ConnectionStringProvider connectionStringProvider, ICmdHelper cmdHelper)
        {
            ConnectionStringProvider = connectionStringProvider;
            CmdHelper = cmdHelper;

            Logger = NullLogger<NewCommand>.Instance;
        }

        protected ProjectBuildArgs GetProjectBuildArgs(CommandLineArgs commandLineArgs, string template, string projectName)
        {
            var version = commandLineArgs.Options.GetOrNull(Options.Version.Short, Options.Version.Long);

            if (version != null)
            {
                Logger.LogInformation("Version: " + version);
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

            IO.DirectoryHelper.CreateIfNotExists(outputFolder);

            Logger.LogInformation("Output folder: " + outputFolder);

            if (connectionString == null &&
                databaseManagementSystem != DatabaseManagementSystem.NotSpecified &&
                databaseManagementSystem != DatabaseManagementSystem.SQLServer)
            {
                connectionString = ConnectionStringProvider.GetByDbms(databaseManagementSystem, outputFolder);
            }

            commandLineArgs.Options.Add(CliConsts.Command, commandLineArgs.Command);

            return new ProjectBuildArgs(
                solutionName,
                template,
                version,
                outputFolder,
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
            );
        }

        protected void ExtractProjectZip(ProjectBuildResult project, string outputFolder)
        {
            using (var templateFileStream = new MemoryStream(project.ZipContent))
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
        }

        protected void OpenRelatedWebPage(ProjectBuildArgs projectArgs,
            string template,
            bool isTiered,
            CommandLineArgs commandLineArgs)
        {
            if (AppTemplateBase.IsAppTemplate(template))
            {
                var isCommercial = template == AppProTemplate.TemplateName;
                OpenThanksPage(projectArgs.UiFramework, projectArgs.DatabaseProvider, isTiered || commandLineArgs.Options.ContainsKey("separate-identity-server"), isCommercial);
            }
            else if (MicroserviceTemplateBase.IsMicroserviceTemplate(template))
            {
                OpenMicroserviceDocumentPage();
            }
        }

        protected string FindMicroserviceSolutionName(string outputFolderRoot)
        {
            var slnFile = Directory.GetFiles(outputFolderRoot, "*.sln").FirstOrDefault();

            if (slnFile == null)
            {
                return null;
            }

            return Path.GetFileName(slnFile).RemovePostFix(".sln");
        }

        protected UiFramework FindMicroserviceSolutionUiFramework(string outputFolderRoot)
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

        protected void OpenThanksPage(UiFramework uiFramework, DatabaseProvider databaseProvider, bool tiered, bool commercial)
        {
            uiFramework = uiFramework == UiFramework.NotSpecified || uiFramework == UiFramework.None ? UiFramework.Mvc : uiFramework;
            databaseProvider = databaseProvider == DatabaseProvider.NotSpecified ? DatabaseProvider.EntityFrameworkCore : databaseProvider;

            var urlPrefix = commercial ? "commercial" : "www";
            var tieredYesNo = tiered ? "yes" : "no";
            var url = $"https://{urlPrefix}.abp.io/project-created-success?ui={uiFramework:g}&db={databaseProvider:g}&tiered={tieredYesNo}";

            CmdHelper.OpenWebPage(url);
        }

        protected void OpenMicroserviceDocumentPage()
        {
            var url = "https://docs.abp.io/en/commercial/latest/startup-templates/microservice/index";

            CmdHelper.OpenWebPage(url);
        }

        protected bool GetCreateSolutionFolderPreference(CommandLineArgs commandLineArgs)
        {
            return commandLineArgs.Options.ContainsKey(Options.CreateSolutionFolder.Long)
                || commandLineArgs.Options.ContainsKey(Options.CreateSolutionFolder.Short);
        }

        protected static string GetConnectionString(CommandLineArgs commandLineArgs)
        {
            var connectionString = commandLineArgs.Options.GetOrNull(Options.ConnectionString.Short, Options.ConnectionString.Long);
            return string.IsNullOrWhiteSpace(connectionString) ? null : connectionString;
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
