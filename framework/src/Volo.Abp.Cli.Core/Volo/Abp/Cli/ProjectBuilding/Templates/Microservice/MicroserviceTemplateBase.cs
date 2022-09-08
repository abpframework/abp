using System.Collections.Generic;
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
        var steps = new List<ProjectBuildPipelineStep>();

        DeleteUnrelatedProjects(context, steps);
        RandomizeStringEncryption(context, steps);
        UpdateNuGetConfig(context, steps);
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

        steps.Add(new ChangeThemeStep());
        RemoveLeptonXThemePackagesFromPackageJsonFiles(steps, uiFramework: context.BuildArgs.UiFramework);
    }

    private static void RemoveLeptonXThemePackagesFromPackageJsonFiles(List<ProjectBuildPipelineStep> steps, UiFramework uiFramework)
    {
        var mvcUiPackageName = "@volo/abp.aspnetcore.mvc.ui.theme.leptonx";
        var packageJsonFilePaths = new List<string> 
        {
            "/MyCompanyName.MyProjectName.AuthServer/package.json",
            "/MyCompanyName.MyProjectName.Web/package.json"
        };

        foreach (var packageJsonFilePath in packageJsonFilePaths)
        {
            steps.Add(new RemoveDependencyFromPackageJsonFileStep(packageJsonFilePath, mvcUiPackageName));
        }

        if (uiFramework == UiFramework.BlazorServer)
        {
            var blazorServerUiPackageName = "@volo/aspnetcore.components.server.leptonxtheme";
            var blazorServerPackageJsonFilePaths = new List<string> 
            {
                "/MyCompanyName.MyProjectName.Blazor/package.json"
            };
            
            foreach (var blazorServerPackageJsonFilePath in blazorServerPackageJsonFilePaths)
            {
                steps.Add(new RemoveDependencyFromPackageJsonFileStep(blazorServerPackageJsonFilePath, mvcUiPackageName));
                steps.Add(new RemoveDependencyFromPackageJsonFileStep(blazorServerPackageJsonFilePath, blazorServerUiPackageName));
            }
        }
        else if (uiFramework == UiFramework.Angular)
        {
            var ngUiPackageName = "@volosoft/abp.ng.theme.lepton-x";
            var angularPackageJsonFilePaths = new List<string> 
            {
                "/angular/package.json"
            };
            
            foreach (var angularPackageJsonFilePath in angularPackageJsonFilePaths)
            {
                steps.Add(new RemoveDependencyFromPackageJsonFileStep(angularPackageJsonFilePath, ngUiPackageName));
                steps.Add(new RemoveDependencyFromPackageJsonFileStep(angularPackageJsonFilePath, "bootstrap-icons"));
            }
        }
    }

    private static void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        //TODO: move common tasks to methods
        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.None:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web", null,
                    "/apps/web/src/MyCompanyName.MyProjectName.Web"));
                steps.Add(new RemoveFolderStep("/apps/web"));
                steps.Add(new RemoveProjectFromTyeStep("web"));
                steps.Add(new RemoveProjectFromPrometheusStep("web"));

                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor", null,
                    "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.ProductService.Blazor",
                    "/services/product/MyCompanyName.MyProjectName.ProductService.sln",
                    "/services/product/src/MyCompanyName.MyProjectName.ProductService.Blazor"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server", null,
                    "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new RemoveFolderStep("/apps/blazor"));
                steps.Add(new RemoveProjectFromTyeStep("blazor"));
                steps.Add(new RemoveProjectFromTyeStep("blazor-server"));

                steps.Add(new RemoveFolderStep("/apps/angular"));
                break;

            case UiFramework.Angular:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web", null,
                    "/apps/web/src/MyCompanyName.MyProjectName.Web"));
                steps.Add(new RemoveFolderStep("/apps/web"));
                steps.Add(new RemoveProjectFromTyeStep("web"));
                steps.Add(new RemoveProjectFromPrometheusStep("web"));

                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor", null,
                    "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.ProductService.Blazor",
                    "/services/product/MyCompanyName.MyProjectName.ProductService.sln",
                    "/services/product/src/MyCompanyName.MyProjectName.ProductService.Blazor"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server", null,
                    "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new RemoveFolderStep("/apps/blazor"));
                steps.Add(new RemoveProjectFromTyeStep("blazor"));
                steps.Add(new RemoveProjectFromTyeStep("blazor-server"));
                
                context.Symbols.Add("ui:angular");
                break;

            case UiFramework.Blazor:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web", null,
                    "/apps/web/src/MyCompanyName.MyProjectName.Web"));
                steps.Add(new RemoveFolderStep("/apps/web"));
                steps.Add(new RemoveFolderStep("/apps/angular"));
                steps.Add(new RemoveProjectFromTyeStep("web"));
                steps.Add(new RemoveProjectFromPrometheusStep("web"));

                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server",
                    "/apps/blazor/MyCompanyName.MyProjectName.Blazor.sln",
                    "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server",
                    null,
                    "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new RemoveProjectFromTyeStep("blazor-server"));
                
                context.Symbols.Add("ui:blazor");
                break;

            case UiFramework.BlazorServer:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Web", null,
                    "/apps/web/src/MyCompanyName.MyProjectName.Web"));
                steps.Add(new RemoveFolderStep("/apps/web"));
                steps.Add(new RemoveFolderStep("/apps/angular"));
                steps.Add(new RemoveProjectFromTyeStep("web"));
                steps.Add(new RemoveProjectFromPrometheusStep("web"));

                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor",
                    "/apps/blazor/MyCompanyName.MyProjectName.Blazor.sln",
                    "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor",
                    null,
                    "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor"));
                steps.Add(new RemoveProjectFromTyeStep("blazor"));

                steps.Add(new TemplateProjectRenameStep("MyCompanyName.MyProjectName.Blazor.Server",
                    "MyCompanyName.MyProjectName.Blazor"));
                steps.Add(new RenameProjectInTyeStep("blazor-server", "blazor"));
                
                context.Symbols.Add("ui:blazor-server");
                break;

            case UiFramework.Mvc:
            case UiFramework.NotSpecified:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor", null,
                    "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server", null,
                    "/apps/blazor/src/MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.ProductService.Blazor",
                    "/services/product/MyCompanyName.MyProjectName.ProductService.sln",
                    "/services/product/src/MyCompanyName.MyProjectName.ProductService.Blazor"));
                steps.Add(new RemoveFolderStep("/apps/blazor"));
                steps.Add(new RemoveProjectFromTyeStep("blazor"));
                steps.Add(new RemoveProjectFromTyeStep("blazor-server"));

                steps.Add(new RemoveFolderStep("/apps/angular"));
                
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
}
