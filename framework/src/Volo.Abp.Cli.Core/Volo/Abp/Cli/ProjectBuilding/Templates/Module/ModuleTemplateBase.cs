using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Module;

public abstract class ModuleTemplateBase : TemplateInfo
{
    protected ModuleTemplateBase([NotNull] string name)
        : base(name)
    {
    }

    public static bool IsModuleTemplate(string templateName)
    {
        return templateName == ModuleTemplate.TemplateName ||
               templateName == ModuleProTemplate.TemplateName;
    }

    public override IEnumerable<ProjectBuildPipelineStep> GetCustomSteps(ProjectBuildContext context)
    {
        var steps = base.GetCustomSteps(context).ToList();

        DeleteUnrelatedProjects(context, steps);
        RandomizeSslPorts(context, steps);
        UpdateNuGetConfig(context, steps);
        RemoveMigrations(context, steps);
        ChangeConnectionString(context, steps);
        CleanupFolderHierarchy(context, steps);

        return steps;
    }

    private void DeleteUnrelatedProjects(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (!context.BuildArgs.ExtraProperties.ContainsKey("no-ui"))
        {
            return;
        }

        steps.Add(new RemoveProjectFromSolutionStep(
            "MyCompanyName.MyProjectName.Web"
        ));

        steps.Add(new RemoveProjectFromSolutionStep(
            "MyCompanyName.MyProjectName.Blazor"
        ));

        steps.Add(new RemoveProjectFromSolutionStep(
            "MyCompanyName.MyProjectName.Blazor.Server"
        ));

        steps.Add(new RemoveProjectFromSolutionStep(
            "MyCompanyName.MyProjectName.Blazor.WebAssembly"
        ));

        steps.Add(new RemoveProjectFromSolutionStep(
            "MyCompanyName.MyProjectName.Blazor.Host",
            projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Blazor.Host"
        ));

        steps.Add(new RemoveProjectFromSolutionStep(
            "MyCompanyName.MyProjectName.Blazor.Host.Client",
            projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Blazor.Host.Client"
        ));

        steps.Add(new RemoveProjectFromSolutionStep(
            "MyCompanyName.MyProjectName.Blazor.Server.Host",
            projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Blazor.Server.Host"
        ));

        steps.Add(new RemoveProjectFromSolutionStep(
            "MyCompanyName.MyProjectName.Web.Host",
            projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Web.Host"
        ));

        steps.Add(new RemoveProjectFromSolutionStep(
            "MyCompanyName.MyProjectName.Web.Unified",
            projectFolderPath: "/aspnet-core/host/MyCompanyName.MyProjectName.Web.Unified"
        ));

        steps.Add(new RemoveFolderStep("/angular"));
    }

    private void RandomizeSslPorts(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        if (context.BuildArgs.ExtraProperties.ContainsKey("no-random-port"))
        {
            return;
        }

        steps.Add(new TemplateRandomSslPortStep(new List<string>
            {
                "https://localhost:44300",
                "https://localhost:44301",
                "https://localhost:44302",
                "https://localhost:44303",
                "https://localhost:44304",
                "https://localhost:44305"
            }));
    }

    private static void UpdateNuGetConfig(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new UpdateNuGetConfigStep("/aspnet-core/NuGet.Config"));
        steps.Add(new UpdateNuGetConfigStep("/NuGet.Config"));
    }

    protected void RemoveMigrations(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new RemoveFolderStep("/aspnet-core/host/MyCompanyName.MyProjectName.AuthServer/Migrations"));
        steps.Add(new RemoveFolderStep("/aspnet-core/host/MyCompanyName.MyProjectName.Blazor.Server.Host/Migrations"));
        steps.Add(new RemoveFolderStep("/aspnet-core/host/MyCompanyName.MyProjectName.Web.Unified/Migrations"));
        if (context.BuildArgs.TemplateName == ModuleProTemplate.TemplateName)
        {
            steps.Add(new RemoveFolderStep("/aspnet-core/host/MyCompanyName.MyProjectName.HttpApi.Host/Migrations"));
        }
    }

    private void ChangeConnectionString(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
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

    private void CleanupFolderHierarchy(ProjectBuildContext context, List<ProjectBuildPipelineStep> steps)
    {
        steps.Add(new MoveFolderStep("/aspnet-core/", "/"));
    }
}
