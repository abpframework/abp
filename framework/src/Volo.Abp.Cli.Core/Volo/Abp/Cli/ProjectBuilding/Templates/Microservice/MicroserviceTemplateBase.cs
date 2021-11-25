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

        return steps;
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
