using System;
using Volo.Abp.Cli.ProjectBuilding.Building.Steps;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;

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

            if (context.Template.Name == AppProTemplate.TemplateName)
            {
                pipeline.Steps.Add(new LicenseCodeReplaceStep());
            }

            pipeline.Steps.Add(new CreateProjectResultZipStep());

            return pipeline;
        }
    }
}
