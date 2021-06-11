using Volo.Abp.Cli.ProjectBuilding.Building.Steps;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.Microservice;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;

namespace Volo.Abp.Cli.ProjectBuilding.Building
{
    public static class TemplateProjectBuildPipelineBuilder
    {
        public static ProjectBuildPipeline Build(ProjectBuildContext context)
        {
            var pipeline = new ProjectBuildPipeline(context);

            pipeline.Steps.Add(new FileEntryListReadStep());

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
                pipeline.Steps.Add(new DatabaseManagementSystemChangeStep()); // todo: move to custom steps?
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
}
