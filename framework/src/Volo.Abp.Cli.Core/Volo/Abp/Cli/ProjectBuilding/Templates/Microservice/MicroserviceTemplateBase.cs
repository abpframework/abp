using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;

public abstract class MicroserviceTemplateBase : TemplateInfo
{
    protected MicroserviceTemplateBase([NotNull] string name)
        : base(name)
    {
    }

    public static bool IsMicroserviceTemplate(string templateName)
    {
        return templateName == MicroserviceProTemplate.TemplateName;
    }

    public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
    {
        var steps = base.GetCustomSteps(context).ToList();

        DeleteUnrelatedProjects(context, steps);
        RandomizeStringEncryption(context, steps);
        RandomizeAuthServerPassPhrase(context, steps);
        UpdateNuGetConfig(context, steps);
        UpdateDockerImages(context, steps);
        ConfigureTheme(context, steps);

        return steps;
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
            return;
        }
        else
        {
            steps.Add(new RemoveFileStep("LeptonXFooter.razor", false));
        }

        steps.Add(new ChangeThemeStep());
        ReplaceLeptonXThemePackagesFromPackageJsonFiles(steps, uiFramework: context.BuildArgs.UiFramework, theme: context.BuildArgs.Theme, version: context.BuildArgs.Version ?? context.TemplateFile.Version);
    }

    private static void ReplaceLeptonXThemePackagesFromPackageJsonFiles(List<ProjectBuildPipelineStep> steps, UiFramework uiFramework, Theme? theme, string version)
    {
        var mvcUiPackageName = "@volo/abp.aspnetcore.mvc.ui.theme.leptonx";
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
            "/MyCompanyName.MyProjectName.AuthServer/package.json",
            "/MyCompanyName.MyProjectName.Web/package.json"
        };

        foreach (var packageJsonFilePath in packageJsonFilePaths)
        {
            steps.Add(new ReplaceDependencyFromPackageJsonFileStep(packageJsonFilePath, mvcUiPackageName, newMvcUiPackageName, version));
        }

        if (uiFramework == UiFramework.BlazorServer || uiFramework == UiFramework.BlazorWebApp)
        {
            var blazorServerUiPackageName = "@volo/aspnetcore.components.server.leptonxtheme";
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
                "/MyCompanyName.MyProjectName.Blazor/package.json"
            };

            foreach (var blazorServerPackageJsonFilePath in blazorServerPackageJsonFilePaths)
            {
                steps.Add(new ReplaceDependencyFromPackageJsonFileStep(blazorServerPackageJsonFilePath, mvcUiPackageName, newMvcUiPackageName, version));
                steps.Add(new ReplaceDependencyFromPackageJsonFileStep(blazorServerPackageJsonFilePath, blazorServerUiPackageName, newBlazorServerUiPackageName, version));
            }
        }
        else if (uiFramework == UiFramework.Angular)
        {
            var ngUiPackageName = "@volosoft/abp.ng.theme.lepton-x";
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

    private static void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (!context.BuildArgs.PublicWebSite)
        {
            steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.PublicWeb", null, "apps/public-web/src/MyCompanyName.MyProjectName.PublicWeb"));
            steps.Add(new RemoveFolderStep("/apps/public-web"));
            steps.Add(new RemoveProjectFromTyeStep("public-web"));
            steps.Add(new RemoveProjectFromPrometheusStep("public-web"));
        }

        //TODO: move common tasks to methods
        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.None:
                DeleteWebProjects(steps);
                DeleteBlazorWebAssemblyProjects(steps);
                DeleteProductServiceBlazorProjects(steps);
                DeleteBlazorServerProjects(steps);
                DeleteBlazorWebAppProjects(steps);
                DeleteAngularProjects(steps);
                steps.Add(new RemoveFolderStep("/apps/blazor"));
                break;

            case UiFramework.Angular:
                DeleteWebProjects(steps);
                DeleteBlazorWebAssemblyProjects(steps);
                DeleteProductServiceBlazorProjects(steps);
                DeleteBlazorServerProjects(steps);
                DeleteBlazorWebAppProjects(steps);
                steps.Add(new RemoveFolderStep("/apps/blazor"));
                context.Symbols.Add("ui:angular");
                break;

            case UiFramework.Blazor:
                DeleteWebProjects(steps);
                DeleteAngularProjects(steps);
                DeleteBlazorServerProjects(steps);
                DeleteBlazorWebAppProjects(steps);
                context.Symbols.Add("ui:blazor");
                break;

            case UiFramework.BlazorServer:
                DeleteWebProjects(steps);
                DeleteAngularProjects(steps);
                DeleteBlazorWebAssemblyProjects(steps);
                DeleteBlazorWebAppProjects(steps);
                
                steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Blazor.Server",
                    "MyCompanyName.MyProjectName.Blazor"));
                steps.Add(new RenameProjectInTyeStep("blazor-server", "blazor"));

                context.Symbols.Add("ui:blazor-server");
                break;
            case UiFramework.BlazorWebApp:
                DeleteWebProjects(steps);
                DeleteAngularProjects(steps);
                DeleteBlazorWebAssemblyProjects(steps);
                DeleteBlazorServerProjects(steps);

                steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Blazor.WebApp",
                    "MyCompanyName.MyProjectName.Blazor"));
                steps.Add(new RenameProjectInTyeStep("blazor-webapp", "blazor"));
                
                context.Symbols.Add("ui:blazor-webapp");
                break;

            case UiFramework.Mvc:
            case UiFramework.NotSpecified:
                DeleteAngularProjects(steps);
                DeleteBlazorWebAssemblyProjects(steps);
                DeleteBlazorServerProjects(steps);
                DeleteBlazorWebAppProjects(steps);
                DeleteProductServiceBlazorProjects(steps);

                context.Symbols.Add("ui:mvc");
                break;
        }

        steps.Add(new RemoveFolderStep("/services/template"));
    }

    private static void RandomizeStringEncryption(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RandomizeStringEncryptionStep());
    }

    private static void UpdateNuGetConfig(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new UpdateNuGetConfigStep("/NuGet.Config"));
    }

    private static void RandomizeAuthServerPassPhrase(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RandomizeAuthServerPassPhraseStep());
    }

    private static void UpdateDockerImages(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new UpdateDockerImagesStep("/etc/docker/docker-compose.infrastructure.yml"));
    }

    private static void DeleteWebProjects(List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web", null, "/apps/web/src/MyCompanyName.MyProjectName.Web"));
        steps.Add(new RemoveFolderStep("/apps/web"));
        steps.Add(new RemoveProjectFromTyeStep("web"));
        steps.Add(new RemoveProjectFromPrometheusStep("web"));
    }
    
    private static void DeleteAngularProjects(List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveFolderStep("/apps/angular"));
    }

    private static void DeleteBlazorWebAssemblyProjects(List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor",
            "/apps/blazor/MyCompanyName.MyProjectName.Blazor.sln",
            "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor",
            null,
            "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor"));

        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Client",
            "/apps/blazor/MyCompanyName.MyProjectName.Blazor.sln",
            "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.Client"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Client",
            null,
            "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.Client"));
        steps.Add(new RemoveProjectFromTyeStep("blazor"));
    }

    private static void DeleteBlazorServerProjects(List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server",
            "/apps/blazor/MyCompanyName.MyProjectName.Blazor.sln",
            "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.Server"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server",
            null,
            "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.Server"));
        steps.Add(new RemoveProjectFromTyeStep("blazor-server"));
    }
    
    private static void DeleteBlazorWebAppProjects(List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp",
            "/apps/blazor/MyCompanyName.MyProjectName.Blazor.sln",
            "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.WebApp"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp",
            null,
            "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.WebApp"));
        
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp.Client",
            "/apps/blazor/MyCompanyName.MyProjectName.Blazor.sln",
            "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.WebApp.Client"));
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebApp.Client",
            null,
            "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.WebApp.Client"));
        
        steps.Add(new RemoveProjectFromTyeStep("blazor-webapp"));
    }
    
    private static void DeleteProductServiceBlazorProjects(List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.ProductService.Blazor",
            "/services/product/MyCompanyName.MyProjectName.ProductService.sln",
            "/services/product/src/MyCompanyName.MyProjectName.ProductService.Blazor"));
    }
}
