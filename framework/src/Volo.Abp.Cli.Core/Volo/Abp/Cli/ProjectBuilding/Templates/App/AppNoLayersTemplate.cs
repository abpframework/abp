using System;
using System.Collections.Generic;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class AppNoLayersTemplate : AppTemplateBase
{
    /// <summary>
    /// "app-nolayers".
    /// </summary>
    public const string TemplateName = "app-nolayers";

    public AppNoLayersTemplate()
        : base(TemplateName)
    {
        //TODO: Change URL
        DocumentUrl = CliConsts.DocsLink + "/en/abp/latest/Startup-Templates/Application";
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

        //ConfigureTenantSchema(context, steps);
        //SwitchDatabaseProvider(context, steps);
        //DeleteUnrelatedProjects(context, steps);
        steps.Add(new RemoveFolderStep("/aspnet-core/src/MyCompanyName.MyProjectName/Migrations"));
        //RemoveMigrations(context, steps);
        //ConfigureTieredArchitecture(context, steps);
        //ConfigurePublicWebSite(context, steps);
        //RemoveUnnecessaryPorts(context, steps);
        RandomizeSslPorts(context, steps);
        RandomizeStringEncryption(context, steps);
        UpdateNuGetConfig(context, steps);
        ChangeConnectionString(context, steps);
        //CleanupFolderHierarchy(context, steps);

        if (context.BuildArgs.UiFramework != UiFramework.Angular)
        {
            steps.Add(new MoveFolderStep("/aspnet-core/", "/"));
        }

        return steps;
    }
}
