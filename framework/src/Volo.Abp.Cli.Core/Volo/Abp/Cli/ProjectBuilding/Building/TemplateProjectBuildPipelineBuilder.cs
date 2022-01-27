using System;
using NuGet.Versioning;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;

namespace Volo.Abp.Cli.ProjectBuilding.Building;

public static class TemplateProjectBuildPipelineBuilder
{
    public static ProjectBuildPipeline Build(ProjectBuildContext context)
    {
        var pipeline = new ProjectBuildPipeline(context);

        pipeline.Steps.Add(new FileEntryListReadStep());

        if (SemanticVersion.Parse(context.TemplateFile.Version) > new SemanticVersion(4, 3, 99))
        {
            pipeline.Steps.Add(new CreateAppSettingsSecretsStep());
        }

        pipeline.Steps.AddRange(context.Template.GetCustomSteps(context));

        pipeline.Steps.Add(new ProjectReferenceReplaceStep());
        pipeline.Steps.Add(new TemplateCodeDeleteStep());
        pipeline.Steps.Add(new SolutionRenameStep());

        if (context.Template.Name == AppProTemplate.TemplateName ||
            context.Template.Name == MicroserviceProTemplate.TemplateName ||
            context.Template.Name == MicroserviceServiceProTemplate.TemplateName ||
            context.Template.Name == ModuleProTemplate.TemplateName)
        {
            pipeline.Steps.Add(new LicenseCodeReplaceStep()); // todo: move to custom steps?
        }

        if (context.Template.Name == AppTemplate.TemplateName ||
            context.Template.Name == AppProTemplate.TemplateName)
        {
            pipeline.Steps.Add(new DatabaseManagementSystemChangeStep(context.Template.As<AppTemplateBase>().HasDbMigrations)); // todo: move to custom steps?
        }

        if (context.Template.Name == AppNoLayersTemplate.TemplateName)
        {
            pipeline.Steps.Add(new AppNoLayersDatabaseManagementSystemChangeStep()); // todo: move to custom steps?
        }

        if ((context.BuildArgs.UiFramework == UiFramework.Mvc || context.BuildArgs.UiFramework == UiFramework.Blazor || context.BuildArgs.UiFramework == UiFramework.BlazorServer)
            && context.BuildArgs.MobileApp == MobileApp.None && context.Template.Name != MicroserviceProTemplate.TemplateName
            && context.Template.Name != MicroserviceServiceProTemplate.TemplateName)
        {
            pipeline.Steps.Add(new RemoveRootFolderStep());
        }

        pipeline.Steps.Add(new CreateProjectResultZipStep());

        return pipeline;
    }
}
