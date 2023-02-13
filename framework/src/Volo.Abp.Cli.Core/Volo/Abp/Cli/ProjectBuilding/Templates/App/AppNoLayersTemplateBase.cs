using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public abstract class AppNoLayersTemplateBase : AppTemplateBase
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
        var steps = new List<ProjectBuildPipelineStep>();

        switch (context.BuildArgs.DatabaseProvider)
        {
            case DatabaseProvider.NotSpecified:
            case DatabaseProvider.EntityFrameworkCore:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc.Mongo"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host.Mongo"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server.Mongo"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.WebAssembly.Server.Mongo", projectFolderPath: "/aspnet-core/MyCompanyName.MyProjectName.Blazor.WebAssembly/Server.Mongo"));
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

        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.Angular:
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
                steps.Add(new RemoveFolderStep("/angular"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Blazor.Server", "MyCompanyName.MyProjectName"));
                RemoveBlazorWasmProjects(steps);
                break;

            case UiFramework.NotSpecified:
            case UiFramework.Mvc:
                steps.Add(new RemoveFolderStep("/angular"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Mvc", "MyCompanyName.MyProjectName"));
                RemoveBlazorWasmProjects(steps);
                break;
            
            default:
                throw new AbpException("Unkown UI framework: " + context.BuildArgs.UiFramework);
        }

        steps.Add(new RemoveFolderStep("/aspnet-core/MyCompanyName.MyProjectName/Migrations"));
        steps.Add(new RemoveFolderStep("/aspnet-core/MyCompanyName.MyProjectName.Host/Migrations"));
        RandomizeSslPorts(context, steps);
        RandomizeStringEncryption(context, steps);
        UpdateNuGetConfig(context, steps);
        ChangeConnectionString(context, steps);
        ConfigureDockerFiles(context, steps);
        ConfigureTheme(context, steps);

        if (context.BuildArgs.UiFramework != UiFramework.Angular)
        {
            steps.Add(new MoveFolderStep("/aspnet-core/", "/"));
        }

        return steps;
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
}
