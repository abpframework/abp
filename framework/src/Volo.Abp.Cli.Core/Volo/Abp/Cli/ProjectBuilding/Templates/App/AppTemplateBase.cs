using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;
using Volo.Abp.Cli.ProjectBuilding.Templates.Maui;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public abstract class AppTemplateBase : TemplateInfo
{
    public bool HasDbMigrations { get; set; }

    protected AppTemplateBase(string templateName)
        : base(templateName, DatabaseProvider.EntityFrameworkCore, UiFramework.Mvc)
    {

    }

    public static bool IsAppTemplate(string templateName)
    {
        return templateName == AppTemplate.TemplateName ||
               templateName == AppProTemplate.TemplateName;
    }

    public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
    {
        var steps = base.GetCustomSteps(context).ToList();

        ConfigureTenantSchema(context, steps);
        SwitchDatabaseProvider(context, steps);
        DeleteUnrelatedProjects(context, steps);
        RemoveMigrations(context, steps);
        ConfigureTieredArchitecture(context, steps);
        ConfigurePublicWebSite(context, steps);
        ConfigureTheme(context, steps);
        ConfigureVersion(context, steps);
        RemoveUnnecessaryPorts(context, steps);
        RandomizeSslPorts(context, steps);
        RandomizeStringEncryption(context, steps);
        RandomizeAuthServerPassPhrase(context, steps);
        UpdateNuGetConfig(context, steps);
        ConfigureDockerFiles(context, steps);
        ChangeConnectionString(context, steps);
        CleanupFolderHierarchy(context, steps);

        return steps;
    }

    protected void ConfigureTenantSchema(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (context.BuildArgs.ExtraProperties.ContainsKey("separate-tenant-schema"))
        {
            if (HasDbMigrations)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations"));
                steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.EntityFrameworkCore.SeparateDbMigrations", "MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations"));
            }
            else
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore"));
                steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.EntityFrameworkCoreWithSeparateDbContext", "MyCompanyName.MyProjectName.EntityFrameworkCore"));
            }
        }
        else
        {
            if (HasDbMigrations)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.SeparateDbMigrations"));
            }
            else
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCoreWithSeparateDbContext"));
            }
        }
    }

    protected void SwitchDatabaseProvider(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (context.BuildArgs.DatabaseProvider == DatabaseProvider.MongoDb)
        {
            steps.Add(new AppTemplateSwitchEntityFrameworkCoreToMongoDbStep(HasDbMigrations));
        }

        if (context.BuildArgs.DatabaseProvider != DatabaseProvider.EntityFrameworkCore)
        {
            if (HasDbMigrations)
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.EntityFrameworkCore.Tests"));
            }
            else
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.EntityFrameworkCore.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.EntityFrameworkCore.Tests"));
            }
        }
        else
        {
            context.Symbols.Add("EFCORE");
            SetDbmsSymbols(context);
        }

        if (context.BuildArgs.DatabaseProvider != DatabaseProvider.MongoDb)
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MongoDB.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.MongoDB.Tests"));
        }

        context.Symbols.Add($"dbms:{context.BuildArgs.DatabaseManagementSystem}");
    }

    protected void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (context.BuildArgs.UiFramework != UiFramework.Blazor)
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Client"));
        }

        if (context.BuildArgs.UiFramework != UiFramework.BlazorServer)
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server.Tiered"));
        }

        if (context.BuildArgs.UiFramework != UiFramework.BlazorWebApp)
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp.Client"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp.Tiered"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp.Tiered.Client"));
        }

        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.None:
                ConfigureWithoutUi(context, steps);
                break;

            case UiFramework.Angular:
                ConfigureWithAngularUi(context, steps);
                break;

            case UiFramework.Blazor:
                ConfigureWithBlazorUi(context, steps);
                break;

            case UiFramework.BlazorServer:
                ConfigureWithBlazorServerUi(context, steps);
                break;

            case UiFramework.BlazorWebApp:
                ConfigureWithBlazorWebAppUi(context, steps);
                break;

            case UiFramework.MauiBlazor:
                ConfigureWithMauiBlazorUi(context, steps);
                break;

            case UiFramework.Mvc:
            case UiFramework.NotSpecified:
                ConfigureWithMvcUi(context, steps);
                break;
        }

        if (context.BuildArgs.UiFramework != UiFramework.MauiBlazor)
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.MauiBlazor"));
        }

        if (context.BuildArgs.UiFramework != UiFramework.Angular)
        {
            steps.Add(new RemoveFolderStep("/angular"));
        }

        if(context.BuildArgs.MobileApp == MobileApp.ReactNative)
        {
            context.Symbols.Add("mobile:react-native");
        }
        else
        {
            steps.Add(new RemoveFolderStep(MobileApp.ReactNative.GetFolderName().EnsureStartsWith('/')));
        }

        if (context.BuildArgs.MobileApp == MobileApp.Maui)
        {
            steps.Add(new MauiChangeApplicationIdGuidStep());
            steps.Add(new MauiChangePortStep());
            context.Symbols.Add("mobile:maui");
        }
        else
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Maui"));
        }

        if (!context.BuildArgs.PublicWebSite)
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Public"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Public.Host"));
        }
        else
        {
            if (context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.Tiered.Long) ||
                context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server") ||
                context.BuildArgs.ExtraProperties.ContainsKey("separate-auth-server"))
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Public"));
                context.Symbols.Add("ui:mvc-public-host");
            }
            else
            {
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Public.Host"));
                context.Symbols.Add("ui:mvc-public");
            }
        }
    }

    protected void ConfigureTheme(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (!context.BuildArgs.Theme.HasValue)
        {
            return;
        }

        if (context.BuildArgs.Theme != Theme.NotSpecified)
        {
            context.Symbols.Add(context.BuildArgs.Theme.Value.ToString().ToUpper());
        }

        if (context.BuildArgs.Theme == Theme.LeptonX)
        {
            steps.Add(new ChangeThemeStyleStep());
        }
        else
        {
            steps.Add(new RemoveFileStep("LeptonXFooter.razor", false));
        }

        RemoveThemeLogoFolders(context, steps);

        if (IsDefaultThemeForTemplate(context.BuildArgs))
        {
            return;
        }

        steps.Add(new ChangeThemeStep());
        ReplaceLeptonXThemePackagesFromPackageJsonFiles(steps, isProTemplate: IsPro(), uiFramework: context.BuildArgs.UiFramework, theme: context.BuildArgs.Theme, version: context.BuildArgs.Version ?? context.TemplateFile.Version);
    }

    protected void SetDbmsSymbols(ProjectBuildContext context)
    {
        switch (context.BuildArgs.DatabaseManagementSystem)
        {
            case DatabaseManagementSystem.NotSpecified:
                context.Symbols.Add("SqlServer");
                break;
            case DatabaseManagementSystem.SQLServer:
                context.Symbols.Add("SqlServer");
                break;
            case DatabaseManagementSystem.MySQL:
                context.Symbols.Add("MySql");
                break;
            case DatabaseManagementSystem.PostgreSQL:
                context.Symbols.Add("PostgreSql");
                break;
            case DatabaseManagementSystem.Oracle:
                context.Symbols.Add("Oracle");
                break;
            case DatabaseManagementSystem.OracleDevart:
                context.Symbols.Add("Oracle");
                break;
            case DatabaseManagementSystem.SQLite:
                context.Symbols.Add("SqLite");
                break;
            default:
                throw new AbpException("Unknown Dbms: " + context.BuildArgs.DatabaseManagementSystem);
        }
    }

    private void RemoveThemeLogoFolders(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (context.BuildArgs.Theme != Theme.Lepton && IsPro())
        {
            steps.Add(new RemoveFilesStep("/wwwroot/images/logo/lepton/"));
        }

        if (context.BuildArgs.Theme != Theme.LeptonX && context.BuildArgs.Theme != Theme.LeptonXLite)
        {
            steps.Add(new RemoveFilesStep("/wwwroot/images/logo/leptonx/"));
        }
    }

    private static bool IsDefaultThemeForTemplate(ProjectBuildArgs args)
    {
        var templateThemes = new Dictionary<string, Theme>
        {
            { AppTemplate.TemplateName, AppTemplate.DefaultTheme },
            { AppProTemplate.TemplateName, AppProTemplate.DefaultTheme }
        };

        return templateThemes.TryGetValue(args.TemplateName!, out var templateTheme) && templateTheme == args.Theme;
    }

    private static void ReplaceLeptonXThemePackagesFromPackageJsonFiles(List<ProjectBuildPipelineStep> steps, bool isProTemplate, UiFramework uiFramework, Theme? theme, string version)
    {
        var mvcUiPackageName = isProTemplate ? "@volo/abp.aspnetcore.mvc.ui.theme.leptonx" : "@abp/aspnetcore.mvc.ui.theme.leptonxlite";
        var newMvcUiPackageName = theme switch
        {
            Theme.Basic => "@abp/aspnetcore.mvc.ui.theme.basic",
            Theme.Lepton => "@volo/abp.aspnetcore.mvc.ui.theme.lepton",
            Theme.LeptonXLite => "@abp/aspnetcore.mvc.ui.theme.leptonxlite",
            Theme.LeptonX => "@volo/abp.aspnetcore.mvc.ui.theme.leptonx",
            _ => throw new AbpException("Unknown theme: " + theme?.ToString())
        };
        if (theme == Theme.LeptonX || theme == Theme.LeptonXLite)
        {
            version = null;
        }
        var packageJsonFilePaths = new List<string>
        {
            "/MyCompanyName.MyProjectName.Web/package.json",
            "/MyCompanyName.MyProjectName.Web.Host/package.json",
            "/MyCompanyName.MyProjectName.Web.Public/package.json",
            "/MyCompanyName.MyProjectName.Web.Public.Host/package.json",
            "/MyCompanyName.MyProjectName.HttpApi.HostWithIds/package.json",
            "/MyCompanyName.MyProjectName.HttpApi.Host/package.json",
            "/MyCompanyName.MyProjectName.AuthServer/package.json",
            "/MyCompanyName.MyProjectName/package.json"
        };

        foreach (var packageJsonFilePath in packageJsonFilePaths)
        {
            steps.Add(new ReplaceDependencyFromPackageJsonFileStep(packageJsonFilePath, mvcUiPackageName, newMvcUiPackageName, version));
        }

        if (uiFramework == UiFramework.BlazorServer || uiFramework == UiFramework.BlazorWebApp)
        {
            var blazorServerUiPackageName = isProTemplate ? "@volo/aspnetcore.components.server.leptonxtheme" : "@abp/aspnetcore.components.server.leptonxlitetheme";
            var newBlazorServerUiPackageName = theme switch
            {
                Theme.Basic => "@abp/aspnetcore.components.server.basictheme",
                Theme.Lepton => "@volo/abp.aspnetcore.components.server.leptontheme",
                Theme.LeptonXLite => "@abp/aspnetcore.components.server.leptonxlitetheme",
                Theme.LeptonX => "@volo/aspnetcore.components.server.leptonxtheme",
                _ => throw new AbpException("Unknown theme: " + theme?.ToString())
            };
            var blazorServerPackageJsonFilePaths = new List<string>
            {
                "/MyCompanyName.MyProjectName.Blazor/package.json",
                "/MyCompanyName.MyProjectName.Blazor.Server.Tiered/package.json"
            };

            foreach (var blazorServerPackageJsonFilePath in blazorServerPackageJsonFilePaths)
            {
                steps.Add(new ReplaceDependencyFromPackageJsonFileStep(blazorServerPackageJsonFilePath, mvcUiPackageName, newMvcUiPackageName, version));
                steps.Add(new ReplaceDependencyFromPackageJsonFileStep(blazorServerPackageJsonFilePath, blazorServerUiPackageName, newBlazorServerUiPackageName, version));
            }
        }
        else if (uiFramework == UiFramework.Angular)
        {
            var ngUiPackageName = isProTemplate ? "@volosoft/abp.ng.theme.lepton-x" : "@abp/ng.theme.lepton-x";
            var newNgUiPackageName = theme switch
            {
                Theme.Basic => "@abp/ng.theme.basic",
                Theme.Lepton => "@volo/abp.ng.theme.lepton",
                Theme.LeptonXLite => "@abp/ng.theme.lepton-x",
                Theme.LeptonX => "@volosoft/abp.ng.theme.lepton-x",
                _ => throw new AbpException("Unknown theme: " + theme?.ToString())
            };
            var angularPackageJsonFilePaths = new List<string>
            {
                "/angular/package.json"
            };

            foreach (var angularPackageJsonFilePath in angularPackageJsonFilePaths)
            {
                steps.Add(new ReplaceDependencyFromPackageJsonFileStep(angularPackageJsonFilePath, ngUiPackageName, newNgUiPackageName, version));
                if (theme == Theme.Basic || theme == Theme.Lepton)
                {
                    steps.Add(new RemoveDependencyFromPackageJsonFileStep(angularPackageJsonFilePath, "bootstrap-icons"));
                }
            }
        }
    }

    protected void ConfigurePublicWebSite(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (!context.BuildArgs.PublicWebSite)
        {
            if (context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.Tiered.Long) ||
                context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server") ||
                context.BuildArgs.ExtraProperties.ContainsKey("separate-auth-server"))
            {
                context.Symbols.Add("PUBLIC-REDIS");
            }
        }
        else
        {
            context.Symbols.Add("PUBLIC-REDIS");
            context.Symbols.Add("public-website");
        }

        if (context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.Tiered.Long) || context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server") || context.BuildArgs.ExtraProperties.ContainsKey("separate-auth-server"))
        {
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Web.Public.Host", "MyCompanyName.MyProjectName.Web.Public"));
            steps.Add(new ChangeDbMigratorPublicPortStep());
        }
        else if (context.BuildArgs.UiFramework != UiFramework.NotSpecified && context.BuildArgs.UiFramework != UiFramework.Mvc)
        {
            steps.Add(new ChangePublicAuthPortStep());
        }

        if (context.BuildArgs.PublicWebSite && !context.BuildArgs.ExtraProperties.ContainsKey("without-cms-kit") && IsCmsKitSupportedForTargetVersion(context))
        {
            context.Symbols.Add("CMS-KIT");
        }
        else
        {
            RemoveCmsKitDependenciesFromPackageJsonFiles(steps);
        }
    }

    protected static void RemoveCmsKitDependenciesFromPackageJsonFiles(List<ProjectBuildPipelineStep> steps)
    {
        var adminCmsPackageInstalledProjectsPackageJsonFiles = new List<string>
            {
                "/aspnet-core/src/MyCompanyName.MyProjectName.Web/package.json",
                "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Host/package.json",
                "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server/package.json",
                "/aspnet-core/src/MyCompanyName.MyProjectName.Blazor.Server.Tiered/package.json"
            };

        var publicCmsPackageInstalledProjectsPackageJsonFiles = new List<string>
            {
                "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Public/package.json",
                "/aspnet-core/src/MyCompanyName.MyProjectName.Web.Public.Host/package.json"
            };

        foreach (var packageJsonFile in adminCmsPackageInstalledProjectsPackageJsonFiles)
        {
            steps.Add(new RemoveDependencyFromPackageJsonFileStep(packageJsonFile, "@volo/cms-kit-pro.admin"));
        }

        foreach (var packageJsonFile in publicCmsPackageInstalledProjectsPackageJsonFiles)
        {
            steps.Add(new RemoveDependencyFromPackageJsonFileStep(packageJsonFile, "@volo/cms-kit-pro.public"));
        }
    }

    protected bool IsCmsKitSupportedForTargetVersion(ProjectBuildContext context)
    {
        if (string.IsNullOrWhiteSpace(context.BuildArgs.Version))
        {
            return true;
        }

        return SemanticVersion.Parse(context.BuildArgs.Version) > SemanticVersion.Parse("4.2.9");
    }

    protected void ConfigureWithoutUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));

        if (context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server") ||
            context.BuildArgs.ExtraProperties.ContainsKey("separate-auth-server"))
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));
            steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));
        }
        else
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.AuthServer"));
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds", "MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44305"));
        }
    }

    protected void ConfigureWithBlazorUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        context.Symbols.Add("ui:blazor");

        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));

        if (context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server") ||
            context.BuildArgs.ExtraProperties.ContainsKey("separate-auth-server"))
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));
            steps.Add(new BlazorAppsettingsFilePortChangeForSeparatedAuthServersStep());
            steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));
        }
        else
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.AuthServer"));
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds", "MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44305"));
        }

        if (context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.ProgressiveWebApp.Short))
        {
            context.Symbols.Add("PWA");
        }
        else
        {
            steps.Add(new RemoveFileStep("/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/wwwroot/service-worker.js"));
            steps.Add(new RemoveFileStep("/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/wwwroot/service-worker.published.js"));
            steps.Add(new RemoveFileStep("/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/wwwroot/manifest.json"));
            steps.Add(new RemoveFileStep("/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/wwwroot/icon-192.png"));
            steps.Add(new RemoveFileStep("/aspnet-core/src/MyCompanyName.MyProjectName.Blazor/wwwroot/icon-512.png"));
        }
    }

    protected void ConfigureWithBlazorServerUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        context.Symbols.Add("ui:blazor-server");

        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));

        if (context.BuildArgs.ExtraProperties.ContainsKey("tiered"))
        {
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Blazor.Server.Tiered", "MyCompanyName.MyProjectName.Blazor"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
            steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));
        }
        else
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server.Tiered"));
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Blazor.Server", "MyCompanyName.MyProjectName.Blazor"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.AuthServer"));
            steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44313"));
        }
    }

    protected void ConfigureWithBlazorWebAppUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        context.Symbols.Add("ui:blazor-webapp");

        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Client"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));

        if (context.BuildArgs.ExtraProperties.ContainsKey("tiered"))
        {
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Blazor.WebApp.Tiered", "MyCompanyName.MyProjectName.Blazor"));
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Blazor.WebApp.Tiered.Client", "MyCompanyName.MyProjectName.Blazor.Client"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp.Client"));
            steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));
        }
        else
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp.Tiered"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp.Tiered.Client"));
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Blazor.WebApp", "MyCompanyName.MyProjectName.Blazor"));
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Blazor.WebApp.Client", "MyCompanyName.MyProjectName.Blazor.Client"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.AuthServer"));
            steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44313"));
        }
    }

    protected void ConfigureWithMvcUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        context.Symbols.Add("ui:mvc");

        if (context.BuildArgs.ExtraProperties.ContainsKey("tiered"))
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Web.Host", "MyCompanyName.MyProjectName.Web"));
            steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));
        }
        else
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.AuthServer"));
            steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44303"));
        }

        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));
    }

    protected void ConfigureWithAngularUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        context.Symbols.Add("ui:angular");

        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));

        if (context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server") ||
            context.BuildArgs.ExtraProperties.ContainsKey("separate-auth-server"))
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));
            steps.Add(new AngularEnvironmentFilePortChangeForSeparatedAuthServersStep());
            steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));

            if (context.BuildArgs.MobileApp == MobileApp.ReactNative)
            {
                steps.Add(new ReactEnvironmentFilePortChangeForSeparatedAuthServersStep());
            }
        }
        else
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.AuthServer"));
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds", "MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44305"));
        }

        if (context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.ProgressiveWebApp.Short))
        {
            context.Symbols.Add("PWA");
        }
    }

    protected void ConfigureWithMauiBlazorUi(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        context.Symbols.Add("ui:maui-blazor");

        steps.Add(new MauiBlazorChangeApplicationIdGuidStep());
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Host"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web.Tests", projectFolderPath: "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests"));
        steps.Add(new RemoveFileStep("/aspnet-core/src/MyCompanyName.MyProjectName.MauiBlazor/Directory.Build.props"));

        if (context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server") ||
            context.BuildArgs.ExtraProperties.ContainsKey("separate-auth-server"))
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds"));
            steps.Add(new MauiBlazorPortChangeForSeparatedAuthServersStep());
            steps.Add(new AppTemplateChangeDbMigratorPortSettingsStep("44300"));

            if (context.BuildArgs.MobileApp == MobileApp.ReactNative)
            {
                steps.Add(new ReactEnvironmentFilePortChangeForSeparatedAuthServersStep());
            }
        }
        else
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.IdentityServer"));
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.AuthServer"));
            steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.HttpApi.HostWithIds", "MyCompanyName.MyProjectName.HttpApi.Host"));
            steps.Add(new AppTemplateChangeConsoleTestClientPortSettingsStep("44305"));
        }
    }

    protected void RemoveUnnecessaryPorts(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveUnnecessaryPortsStep());
    }

    protected void ConfigureVersion(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (context.BuildArgs.Version == null || SemanticVersion.Parse(context.BuildArgs.Version) >= SemanticVersion.Parse("6.0.0-rc.1"))
        {
            context.Symbols.Add("newer-than-6.0");
        }
    }

    protected void RandomizeSslPorts(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (context.BuildArgs.ExtraProperties.ContainsKey("no-random-port"))
        {
            return;
        }

        //todo: discuss blazor ports
        steps.Add(new TemplateRandomSslPortStep(
                new List<string>
                {
                        "https://localhost:44300",
                        "https://localhost:44301",
                        "https://localhost:44302",
                        "https://localhost:44303",
                        "https://localhost:44304",
                        "https://localhost:44305",
                        "https://localhost:44306",
                        "https://localhost:44307",
                        "https://localhost:44308",
                        "https://localhost:44309",
                        "https://localhost:44310"
                }
            )
        );
    }

    protected void ConfigureTieredArchitecture(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (context.BuildArgs.ExtraProperties.ContainsKey(NewCommand.Options.Tiered.Long) ||
            context.BuildArgs.ExtraProperties.ContainsKey("separate-identity-server") ||
            context.BuildArgs.ExtraProperties.ContainsKey("separate-auth-server"))
        {
            context.Symbols.Add("tiered");
        }
    }

    protected void RandomizeStringEncryption(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RandomizeStringEncryptionStep());
    }

    protected static void RandomizeAuthServerPassPhrase(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RandomizeAuthServerPassPhraseStep());
    }

    protected void UpdateNuGetConfig(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new UpdateNuGetConfigStep("/aspnet-core/NuGet.Config"));
    }

    protected void RemoveMigrations(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (string.IsNullOrWhiteSpace(context.BuildArgs.Version) ||
            SemanticVersion.Parse(context.BuildArgs.Version) > new SemanticVersion(4, 1, 99))
        {
            if (HasDbMigrations)
            {
                steps.Add(new RemoveFolderStep("/aspnet-core/src/MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations/Migrations"));
                steps.Add(new RemoveFolderStep("/aspnet-core/src/MyCompanyName.MyProjectName.EntityFrameworkCore.DbMigrations/TenantMigrations"));
            }
            else
            {
                steps.Add(new RemoveFolderStep("/aspnet-core/src/MyCompanyName.MyProjectName.EntityFrameworkCore/Migrations"));
                steps.Add(new RemoveFolderStep("/aspnet-core/src/MyCompanyName.MyProjectName.EntityFrameworkCore/TenantMigrations"));
            }
        }
    }

    protected void ChangeConnectionString(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (context.BuildArgs.ConnectionString != null)
        {
            steps.Add(new ConnectionStringChangeStep());
        }

        if (IsPro())
        {
            steps.Add(new ConnectionStringRenameStep());
        }
    }

    protected void CleanupFolderHierarchy(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if ((context.BuildArgs.UiFramework == UiFramework.Mvc
             || context.BuildArgs.UiFramework == UiFramework.Blazor
             || context.BuildArgs.UiFramework == UiFramework.BlazorServer
             || context.BuildArgs.UiFramework == UiFramework.BlazorWebApp
             || context.BuildArgs.UiFramework == UiFramework.MauiBlazor) &&
            context.BuildArgs.MobileApp == MobileApp.None)
        {
            steps.Add(new MoveFolderStep("/aspnet-core/", "/"));
        }
    }

    private void ConfigureDockerFiles(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.None:
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.Server.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Mvc.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/dynamic-env.json"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/appsettings.json"));
                steps.Add(new MoveFileStep("/aspnet-core/etc/docker/docker-compose.Angular.yml", "/aspnet-core/etc/docker/docker-compose.yml"));
                break;
            case UiFramework.Angular:
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.Server.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Mvc.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/appsettings.json"));
                steps.Add(new MoveFileStep("/aspnet-core/etc/docker/docker-compose.Angular.yml", "/aspnet-core/etc/docker/docker-compose.yml"));
                break;
            case UiFramework.Blazor:
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Angular.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.Server.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Mvc.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/dynamic-env.json"));
                steps.Add(new MoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.yml", "/aspnet-core/etc/docker/docker-compose.yml"));
                break;
            case UiFramework.BlazorServer:
            case UiFramework.BlazorWebApp:
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Angular.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Mvc.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/dynamic-env.json"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/appsettings.json"));
                steps.Add(new MoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.Server.yml", "/aspnet-core/etc/docker/docker-compose.yml"));
                break;
            case UiFramework.NotSpecified:
            case UiFramework.Mvc:
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.Server.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Angular.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/dynamic-env.json"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/appsettings.json"));
                steps.Add(new MoveFileStep("/aspnet-core/etc/docker/docker-compose.Mvc.yml", "/aspnet-core/etc/docker/docker-compose.yml"));
                break;
        }
    }
}
