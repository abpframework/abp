﻿using System.Collections.Generic;
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
                break;
            case DatabaseProvider.MongoDb:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));

                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Mvc.Mongo", "MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Host.Mongo", "MyCompanyName.MyProjectName.Host"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Blazor.Server.Mongo", "MyCompanyName.MyProjectName.Blazor.Server"));
                break;
        }

        context.Symbols.Add($"dbms:{context.BuildArgs.DatabaseManagementSystem}");

        switch (context.BuildArgs.UiFramework)
        {
            case UiFramework.Angular:
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Host", "MyCompanyName.MyProjectName"));
                break;

            case UiFramework.None:
                steps.Add(new RemoveFolderStep("/angular"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Host", "MyCompanyName.MyProjectName"));
                break;

            case UiFramework.BlazorServer:
                steps.Add(new RemoveFolderStep("/angular"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Mvc"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Blazor.Server", "MyCompanyName.MyProjectName"));
                break;

            case UiFramework.NotSpecified:
            case UiFramework.Mvc:
                steps.Add(new RemoveFolderStep("/angular"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Host"));
                steps.Add(new RemoveProjectFromSolutionStep("MyCompanyName.MyProjectName.Blazor.Server"));
                steps.Add(new ProjectRenameStep("MyCompanyName.MyProjectName.Mvc", "MyCompanyName.MyProjectName"));
                break;

            case UiFramework.Blazor:
                throw new AbpException("app-nolayers doesn't support blazor wasm.");
                break;
        }

        steps.Add(new RemoveFolderStep("/aspnet-core/MyCompanyName.MyProjectName/Migrations"));
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
