using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NuGet.Versioning;
using NUglify.Helpers;
using Volo.Abp.Cli.ProjectModification;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Bundling;
using Volo.Abp.Cli.Commands.Services;
using Volo.Abp.Cli.LIbs;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Events;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;
using Volo.Abp.Cli.ProjectBuilding.Templates.Module;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;
using Volo.Abp.Cli.Utils;
using Volo.Abp.EventBus.Local;

namespace Volo.Abp.Cli.Commands;

public abstract class ProjectCreationCommandBase
{
    private readonly IBundlingService _bundlingService;
    public ConnectionStringProvider ConnectionStringProvider { get; }
    public SolutionPackageVersionFinder SolutionPackageVersionFinder { get; }
    public ICmdHelper CmdHelper { get; }
    public IInstallLibsService InstallLibsService { get; }
    public CliService CliService { get; }
    public AngularPwaSupportAdder AngularPwaSupportAdder { get; }
    public InitialMigrationCreator InitialMigrationCreator { get; }
    public ILocalEventBus EventBus { get; }
    public ILogger<NewCommand> Logger { get; set; }

    public ThemePackageAdder ThemePackageAdder { get; }
    
    public AngularThemeConfigurer AngularThemeConfigurer { get; }

    public ProjectCreationCommandBase(
        ConnectionStringProvider connectionStringProvider,
        SolutionPackageVersionFinder solutionPackageVersionFinder,
        ICmdHelper cmdHelper,
        IInstallLibsService installLibsService,
        CliService cliService,
        AngularPwaSupportAdder angularPwaSupportAdder,
        InitialMigrationCreator initialMigrationCreator,
        ThemePackageAdder themePackageAdder,
        ILocalEventBus eventBus,
        IBundlingService bundlingService, 
        AngularThemeConfigurer angularThemeConfigurer)
    {
        _bundlingService = bundlingService;
        ConnectionStringProvider = connectionStringProvider;
        SolutionPackageVersionFinder = solutionPackageVersionFinder;
        CmdHelper = cmdHelper;
        InstallLibsService = installLibsService;
        CliService = cliService;
        AngularPwaSupportAdder = angularPwaSupportAdder;
        InitialMigrationCreator = initialMigrationCreator;
        EventBus = eventBus;
        ThemePackageAdder = themePackageAdder;
        AngularThemeConfigurer = angularThemeConfigurer;

        Logger = NullLogger<NewCommand>.Instance;
    }

    protected async Task<ProjectBuildArgs> GetProjectBuildArgsAsync(CommandLineArgs commandLineArgs, string template, string projectName)
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

            var cliVersion = await CliService.GetCurrentCliVersionAsync(typeof(CliService).Assembly);

            if (!cliVersion.IsPrerelease)
            {
                throw new CliUsageException(
                    "You can only create a new preview solution with preview CLI version." +
                    " Update your ABP CLI to the preview version.");
            }
        }

        var pwa = commandLineArgs.Options.ContainsKey(Options.ProgressiveWebApp.Short);
        if (pwa)
        {
            Logger.LogInformation("Progressive Web App: yes");
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

        var uiFramework = GetUiFramework(commandLineArgs, template);
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
            var slnPath = commandLineArgs.Options.GetOrNull(Options.MainSolution.Short, Options.MainSolution.Long);

            if (slnPath == null)
            {
                slnPath = Directory.GetFiles(outputFolderRoot, "*.sln").FirstOrDefault();
            }
            else if (slnPath.EndsWith(".sln"))
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(slnPath));
                outputFolderRoot = Path.GetDirectoryName(slnPath);
            }
            else if (!Directory.Exists(slnPath))
            {
                slnPath = null;
            }
            else
            {
                Directory.SetCurrentDirectory(slnPath);
                outputFolderRoot = slnPath;
                slnPath = Directory.GetFiles(outputFolderRoot, "*.sln").FirstOrDefault();
            }

            if (slnPath == null)
            {
                throw new CliUsageException($"This command should be run inside a folder that contains a microservice solution! Or use -{Options.MainSolution.Short} parameter.");
            }

            var microserviceSolutionName = Path.GetFileName(slnPath).RemovePostFix(".sln");

            version ??= SolutionPackageVersionFinder.Find(slnPath);
            solutionName = SolutionName.Parse(microserviceSolutionName, projectName);
            outputFolder = MicroserviceServiceTemplateBase.CalculateTargetFolder(outputFolderRoot, solutionName.ProjectName);
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
        
        var theme = uiFramework == UiFramework.None ? (Theme?)null : GetThemeByTemplateOrNull(commandLineArgs, template);
        var themeStyle = theme.HasValue ? GetThemeStyleOrNull(commandLineArgs, theme.Value) : (ThemeStyle?)null;

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
            connectionString,
            pwa,
            theme,
            themeStyle
        );
    }

    protected void ExtractProjectZip(ProjectBuildResult project, string outputFolder)
    {
        EventBus.PublishAsync(new ProjectCreationProgressEvent
        {
            Message = "Unzipping the solution"
        }, false);

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

#if DEBUG
        return;
#endif
        if (AppTemplateBase.IsAppTemplate(template))
        {
            var isCommercial = template == AppProTemplate.TemplateName;
            OpenThanksPage(projectArgs.UiFramework, projectArgs.DatabaseProvider, isTiered || commandLineArgs.Options.ContainsKey("separate-identity-server") || commandLineArgs.Options.ContainsKey("separate-auth-server"), isCommercial);
        }
        else if (MicroserviceTemplateBase.IsMicroserviceTemplate(template))
        {
            OpenMicroserviceDocumentPage();
        }
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

        CmdHelper.Open(url);
    }

    protected void OpenMicroserviceDocumentPage()
    {
        var url = "https://docs.abp.io/en/commercial/latest/startup-templates/microservice/index";

        CmdHelper.Open(url);
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

        if (optionValue == null)
        {
            return DatabaseProvider.NotSpecified;
        }

        if (optionValue.Equals("ef", StringComparison.InvariantCultureIgnoreCase) || optionValue.Equals("entityframeworkcore", StringComparison.InvariantCultureIgnoreCase))
        {
            return DatabaseProvider.EntityFrameworkCore;
        }

        if (optionValue.Equals("mongo", StringComparison.InvariantCultureIgnoreCase) || optionValue.Equals("mongodb", StringComparison.InvariantCultureIgnoreCase))
        {
            return DatabaseProvider.MongoDb;
        }

        throw new CliUsageException(ExceptionMessageHelper.GetInvalidOptionExceptionMessage("Database Provider"));
    }

    protected virtual async Task RunGraphBuildForMicroserviceServiceTemplate(ProjectBuildArgs projectArgs)
    {
        if (MicroserviceServiceTemplateBase.IsMicroserviceServiceTemplate(projectArgs.TemplateName))
        {
            await EventBus.PublishAsync(new ProjectCreationProgressEvent
            {
                Message = "Building the microservice solution"
            }, false);

            CmdHelper.RunCmd("dotnet build /graphbuild", projectArgs.OutputFolder);
        }
    }

    protected async Task RunInstallLibsForWebTemplateAsync(ProjectBuildArgs projectArgs)
    {
        if (AppTemplateBase.IsAppTemplate(projectArgs.TemplateName) ||
            ModuleTemplateBase.IsModuleTemplate(projectArgs.TemplateName) ||
            AppNoLayersTemplateBase.IsAppNoLayersTemplate(projectArgs.TemplateName) ||
            MicroserviceServiceTemplateBase.IsMicroserviceTemplate(projectArgs.TemplateName))
        {
            Logger.LogInformation("Installing client-side packages...");

            await EventBus.PublishAsync(new ProjectCreationProgressEvent
            {
                Message = "Installing client-side packages"
            }, false);

            await InstallLibsService.InstallLibsAsync(projectArgs.OutputFolder);
        }
    }

    protected async Task RunBundleForBlazorWasmOrMauiBlazorTemplateAsync(ProjectBuildArgs projectArgs)
    {
        if (AppTemplateBase.IsAppTemplate(projectArgs.TemplateName) && projectArgs.UiFramework is UiFramework.Blazor or UiFramework.MauiBlazor)
        {
            var isWebassembly = projectArgs.UiFramework == UiFramework.Blazor;
            var message = isWebassembly ? "Generating bundles for Blazor Wasm" : "Generating bundles for MAUI Blazor";
            Logger.LogInformation($"{message}...");

            await EventBus.PublishAsync(new ProjectCreationProgressEvent
            {
                Message = message
            }, false);

            var directory = Path.GetDirectoryName(
                Directory.GetFiles(projectArgs.OutputFolder, isWebassembly? "*.Blazor.csproj" :"*.MauiBlazor.csproj", SearchOption.AllDirectories).First()
                );
            
            await _bundlingService.BundleAsync(directory, true, projectType: isWebassembly ? BundlingConsts.WebAssembly : BundlingConsts.MauiBlazor);
        }
    }

    protected async Task CreateInitialMigrationsAsync(ProjectBuildArgs projectArgs)
    {
        if (projectArgs.DatabaseProvider == DatabaseProvider.MongoDb)
        {
            return;
        }

        var efCoreProjectPath = string.Empty;
        bool isLayeredTemplate;

        switch (projectArgs.TemplateName)
        {
            case AppTemplate.TemplateName:
            case AppProTemplate.TemplateName:
                efCoreProjectPath = Directory.GetFiles(projectArgs.OutputFolder, "*EntityFrameworkCore.csproj", SearchOption.AllDirectories).FirstOrDefault();
                isLayeredTemplate = true;
                break;
            case AppNoLayersTemplate.TemplateName:
            case AppNoLayersProTemplate.TemplateName:
                efCoreProjectPath = Directory.GetFiles(projectArgs.OutputFolder, "*.csproj", SearchOption.AllDirectories).FirstOrDefault();
                isLayeredTemplate = false;
                break;
            default:
                return;
        }

        if (string.IsNullOrWhiteSpace(efCoreProjectPath))
        {
            Logger.LogWarning("Couldn't find the project to create initial migrations!");
            return;
        }

        await EventBus.PublishAsync(new ProjectCreationProgressEvent
        {
            Message = "Creating the initial DB migration"
        }, false);

        await InitialMigrationCreator.CreateAsync(Path.GetDirectoryName(efCoreProjectPath), isLayeredTemplate);
    }

    protected async Task ConfigurePwaSupportForAngular(ProjectBuildArgs projectArgs)
    {
        var isAngular = projectArgs.UiFramework == UiFramework.Angular;
        var isPwa = projectArgs.Pwa;

        if (isAngular && isPwa)
        {
            Logger.LogInformation("Adding PWA Support to Angular app.");

            AngularPwaSupportAdder.AddPwaSupport(projectArgs.OutputFolder);
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
                throw new CliUsageException(ExceptionMessageHelper.GetInvalidOptionExceptionMessage("Database Management System"));
        }
    }

    protected virtual MobileApp GetMobilePreference(CommandLineArgs commandLineArgs)
    {
        var optionValue = commandLineArgs.Options.GetOrNull(Options.Mobile.Short, Options.Mobile.Long);

        switch (optionValue)
        {
            case null:
            case "none":
                return MobileApp.None;
            case "react-native":
                return MobileApp.ReactNative;
            case "maui":
                return MobileApp.Maui;
            default:
                throw new CliUsageException(ExceptionMessageHelper.GetInvalidOptionExceptionMessage("Mobile App"));
        }
    }

    protected virtual UiFramework GetUiFramework(CommandLineArgs commandLineArgs, string template = "app")
    {
        if (commandLineArgs.Options.ContainsKey("no-ui"))
        {
            return UiFramework.None;
        }

        var optionValue = commandLineArgs.Options.GetOrNull(Options.UiFramework.Short, Options.UiFramework.Long);

        switch (optionValue)
        {
            case null:
                return UiFramework.NotSpecified;
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
            case "maui-blazor" when template == AppProTemplate.TemplateName:
                return UiFramework.MauiBlazor;
            default:
                throw new CliUsageException(ExceptionMessageHelper.GetInvalidOptionExceptionMessage("UI Framework"));
        }
    }

    protected virtual Theme? GetThemeByTemplateOrNull(CommandLineArgs commandLineArgs, string template = "app")
    {
        var theme = commandLineArgs.Options.GetOrNull(Options.Theme.Long)?.ToLower();

        return template switch
        {
            AppTemplate.TemplateName or AppNoLayersTemplate.TemplateName => GetAppTheme(),
            AppProTemplate.TemplateName or AppNoLayersProTemplate.TemplateName or MicroserviceProTemplate.TemplateName => GetAppProTheme(),
            _ => null
        };

        Theme GetAppTheme()
        {
            return theme switch
            {
                // null or "leptonx-lite" => Theme.LeptonXLite,
                "basic" => Theme.Basic,
                _ => Theme.LeptonXLite 
            };
        }
    
        Theme GetAppProTheme()
        {
            return theme switch
            {
                // null or "leptonx" => Theme.LeptonX,
                "lepton" => Theme.Lepton,
                "basic" => Theme.Basic,
                _ => Theme.LeptonX //TODO: default???
            };
        }
    }

    protected virtual ThemeStyle? GetThemeStyleOrNull(CommandLineArgs commandLineArgs, Theme theme) 
    {
        if(theme != Theme.LeptonX) 
        {
            return null;
        }

        var themeStyle = commandLineArgs.Options.GetOrNull(Options.ThemeStyle.Long)?.ToLower();
        
        return themeStyle switch 
        {
            "system" or null => ThemeStyle.System,
            "dim" => ThemeStyle.Dim,
            "light" => ThemeStyle.Light,
            "dark" => ThemeStyle.Dark,
            _ => null
        };
    }

    protected void ConfigureNpmPackagesForTheme(ProjectBuildArgs projectArgs)
    {
        if (!projectArgs.Theme.HasValue)
        {
            return;
        }

        switch (projectArgs.Theme)
        {
            case Theme.Basic:
                ConfigureNpmPackagesForBasicTheme(projectArgs);
                break;
            case Theme.Lepton:
                ConfigureNpmPackagesForLeptonTheme(projectArgs);
                break;
            case Theme.NotSpecified:
            case Theme.LeptonXLite:
            case Theme.LeptonX:
                break;
            default:
                 throw new CliUsageException(ExceptionMessageHelper.GetInvalidOptionExceptionMessage(Options.Theme.Long));
        }
    }

    private void ConfigureNpmPackagesForBasicTheme(ProjectBuildArgs projectArgs)
    {
        if (projectArgs.UiFramework is not UiFramework.None or UiFramework.Angular)
        {
            ThemePackageAdder.AddNpmPackage(projectArgs.OutputFolder, "@abp/aspnetcore.mvc.ui.theme.basic", projectArgs.Version);
        }

        if (projectArgs.UiFramework is UiFramework.BlazorServer)
        {
            ThemePackageAdder.AddNpmPackage(projectArgs.OutputFolder, "@abp/aspnetcore.components.server.basictheme", projectArgs.Version);
        }

        if (projectArgs.UiFramework is UiFramework.Angular)
        {
            ThemePackageAdder.AddAngularPackage(projectArgs.OutputFolder, "@abp/ng.theme.basic", projectArgs.Version);
        }
    }
    
    private void ConfigureNpmPackagesForLeptonTheme(ProjectBuildArgs projectArgs)
    {
        if (projectArgs.UiFramework is not UiFramework.None or UiFramework.Angular)
        {
            ThemePackageAdder.AddNpmPackage(projectArgs.OutputFolder, "@volo/abp.aspnetcore.mvc.ui.theme.lepton", projectArgs.Version);
        }

        if (projectArgs.UiFramework is UiFramework.BlazorServer)
        {
            ThemePackageAdder.AddNpmPackage(projectArgs.OutputFolder, "@volo/abp.aspnetcore.components.server.leptontheme", projectArgs.Version);
            ThemePackageAdder.AddNpmPackage(projectArgs.OutputFolder, "@volo/abp.aspnetcore.mvc.ui.theme.lepton", projectArgs.Version);
        }

        if (projectArgs.UiFramework is UiFramework.Angular)
        {
            ThemePackageAdder.AddAngularPackage(projectArgs.OutputFolder, "@volo/abp.ng.theme.lepton", projectArgs.Version);
        }
    }

    protected void ConfigureAngularJsonForThemeSelection(ProjectBuildArgs projectArgs)
    {
        if (projectArgs.TemplateName == ModuleTemplate.TemplateName)
        {
            return;
        }
                
        if (projectArgs.Theme.HasValue && projectArgs.UiFramework == UiFramework.Angular)
        {
            var angularFolderPath = projectArgs.TemplateName == MicroserviceProTemplate.TemplateName
                ? projectArgs.OutputFolder.EnsureEndsWith(Path.DirectorySeparatorChar) + "apps" + Path.DirectorySeparatorChar + "angular"
                : projectArgs.OutputFolder.EnsureEndsWith(Path.DirectorySeparatorChar) + "angular";

            AngularThemeConfigurer.Configure(new AngularThemeConfigurationArgs(
                theme: projectArgs.Theme.Value,
                projectName: projectArgs.SolutionName.FullName,
                angularFolderPath: angularFolderPath
            ));
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

        public static class SkipInstallingLibs
        {
            public const string Short = "sib";
            public const string Long = "skip-installing-libs";
        }

        public static class SkipBundling
        {
            public const string Short = "sb";
            public const string Long = "skip-bundling";
        }

        public static class Tiered
        {
            public const string Long = "tiered";
        }

        public static class Preview
        {
            public const string Long = "preview";
        }

        public static class ProgressiveWebApp
        {
            public const string Short = "pwa";
        }

        public static class MainSolution
        {
            public const string Short = "ms";
            public const string Long = "main-solution";
        }

        public static class Theme
        {
            public const string Long = "theme";
        }

        public static class ThemeStyle
        {
            public const string Long = "theme-style";
        }
    }
}
