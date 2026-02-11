using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public abstract class AppNoLayersTemplateBase : TemplateInfo
{
    protected AppNoLayersTemplateBase(string templateName)
        : base(templateName)
    {

    }

    public static bool IsAppNoLayersTemplate(string templateName)
    {
        return templateName == AppNoLayersTemplate.TemplateName ||
               templateName == AppNoLayersProTemplate.TemplateName;
    }

    public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
    {
        var steps = base.GetCustomSteps(context).ToList();

        SwitchDatabaseProvider(context, steps);
        DeleteUnrelatedProjects(context, steps);
        RemoveMigrations(context, steps);
        RandomizeSslPorts(context, steps);
        RandomizeStringEncryption(context, steps);
        RandomizeAuthServerPassPhrase(context, steps);
        UpdateNuGetConfig(context, steps);
        ChangeConnectionString(context, steps);
        ConfigureDockerFiles(context, steps);
        ConfigureTheme(context, steps);
        CleanupFolderHierarchy(context, steps);

        return steps;
    }

    protected void SwitchDatabaseProvider(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        switch (context.BuildArgs.DatabaseProvider)
        {
            case DatabaseProvider.NotSpecified:
            case DatabaseProvider.EntityFrameworkCore:
                context.Symbols.Add("EFCORE");
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc.Mongo"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host.Mongo"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server.Mongo"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebAssembly.Server.Mongo", projectFolderPath: "/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Server.Mongo"));
                SetDbmsSymbols(context);
                break;
            case DatabaseProvider.MongoDb:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebAssembly.Server", projectFolderPath: "/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Server"));

                steps.Add(new MoveFolderStep("/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Server.Mongo", "/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Server"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Mvc.Mongo", "MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Host.Mongo", "MyCompanyName.MyProjectName.Host"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Blazor.Server.Mongo", "MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Blazor.WebAssembly.Server.Mongo", "MyCompanyName.MyProjectName.Blazor.WebAssembly.Server"));
                break;
        }

        context.Symbols.Add($"dbms:{context.BuildArgs.DatabaseManagementSystem}");
    }

    protected void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.Angular:
                context.Symbols.Add("ui:angular");
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Host", "MyCompanyName.MyProjectName"));
                RemoveBlazorWasmProjects(steps);
                break;

            case UiFramework.None:
                steps.Add(new RemoveFolderStep("/angular"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Host", "MyCompanyName.MyProjectName"));
                RemoveBlazorWasmProjects(steps);
                break;

            case UiFramework.Blazor:
            case UiFramework.BlazorWebApp:
                context.Symbols.Add("ui:blazor");
                steps.Add(new RemoveFolderStep("/angular"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host"));

                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Blazor.WebAssembly.Server",
                     "MyCompanyName.MyProjectName.Host"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Blazor.WebAssembly.Client",
                     "MyCompanyName.MyProjectName.Blazor"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Blazor.WebAssembly.Shared",
                    "MyCompanyName.MyProjectName.Contracts"));

                steps.Add(new AppNoLayersMoveProjectsStep());
                steps.Add(new AppNoLayersMigrateDatabaseChangeStep());
                steps.Add(new RemoveFolderStep("/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly"));
                break;

            case UiFramework.BlazorServer:
                context.Symbols.Add("ui:blazor-server");
                steps.Add(new RemoveFolderStep("/angular"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Blazor.Server", "MyCompanyName.MyProjectName"));
                RemoveBlazorWasmProjects(steps);
                break;

            case UiFramework.NotSpecified:
            case UiFramework.Mvc:
                context.Symbols.Add("ui:mvc");
                steps.Add(new RemoveFolderStep("/angular"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Mvc", "MyCompanyName.MyProjectName"));
                RemoveBlazorWasmProjects(steps);
                break;

            default:
                throw new AbpException("Unkown UI framework: " + context.BuildArgs.UiFramework);
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

    protected void CleanupFolderHierarchy(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (context.BuildArgs.UiFramework != UiFramework.Angular)
        {
            steps.Add(new MoveFolderStep("/aspnet-core/", "/"));
        }
    }

    protected void RemoveMigrations(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveFolderStep("/aspnet-core/MyCompanyName.MyProjectName/Migrations"));
        steps.Add(new RemoveFolderStep("/aspnet-core/MyCompanyName.MyProjectName.Host/Migrations"));
    }

    protected void ConfigureDockerFiles(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.None:
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.Server.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Mvc.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/dynamic-env.json"));
                steps.Add(new MoveFileStep("/aspnet-core/etc/docker/docker-compose.Host.yml", "/aspnet-core/etc/docker/docker-compose.yml"));
                break;
            case UiFramework.Angular:
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.Server.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Mvc.yml"));
                steps.Add(new MoveFileStep("/aspnet-core/etc/docker/docker-compose.Host.yml", "/aspnet-core/etc/docker/docker-compose.yml"));
                break;
            case UiFramework.BlazorServer:
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Host.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Mvc.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/dynamic-env.json"));
                steps.Add(new MoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.Server.yml", "/aspnet-core/etc/docker/docker-compose.yml"));
                break;
            case UiFramework.NotSpecified:
            case UiFramework.Mvc:
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Blazor.Server.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/docker-compose.Host.yml"));
                steps.Add(new RemoveFileStep("/aspnet-core/etc/docker/dynamic-env.json"));
                steps.Add(new MoveFileStep("/aspnet-core/etc/docker/docker-compose.Mvc.yml", "/aspnet-core/etc/docker/docker-compose.yml"));
                break;
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

        if (context.BuildArgs.Theme != Theme.LeptonX)
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

    private static void RemoveBlazorWasmProjects(List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebAssembly.Server",
            projectFolderPath: "/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Server"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebAssembly.Client",
            projectFolderPath: "/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Client"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebAssembly.Shared",
            projectFolderPath: "/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Shared"));
        steps.Add(new RemoveFolderStep("/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly"));
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

    private static bool IsDefaultThemeForTemplate(ProjectBuildArgs args)
    {
        var templateThemes = new Dictionary<string, Theme>
        {
            { AppNoLayersTemplate.TemplateName, AppNoLayersTemplate.DefaultTheme },
            { AppNoLayersProTemplate.TemplateName, AppNoLayersProTemplate.DefaultTheme }
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
            "/MyCompanyName.MyProjectName/package.json",
            "/MyCompanyName.MyProjectName.Host/package.json",
            "/MyCompanyName.MyProjectName.Host.Mongo/package.json"
        };

        foreach (var packageJsonFilePath in packageJsonFilePaths)
        {
            steps.Add(new ReplaceDependencyFromPackageJsonFileStep(packageJsonFilePath, mvcUiPackageName, newMvcUiPackageName, version));
        }

        if (uiFramework == UiFramework.BlazorServer)
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
                "/MyCompanyName.MyProjectName/package.json",
                "/MyCompanyName.MyProjectName.Blazor/package.json",
                "/MyCompanyName.MyProjectName.Blazor.Server.Mongo/package.json"
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
}
