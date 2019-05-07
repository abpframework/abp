using Volo.Abp.ProjectBuilding.Building.Steps;

namespace Volo.Abp.ProjectBuilding.Building
{
    public static class ProjectBuildPipelineBuilder
    {
        public static ProjectBuildPipeline Build(ProjectBuildContext context)
        {
            var pipeline = new ProjectBuildPipeline();

            pipeline.Steps.Add(new FileEntryListReadStep());
            pipeline.Steps.AddRange(context.Template.GetCustomSteps(context));
            pipeline.Steps.Add(new NugetReferenceReplaceStep());
            pipeline.Steps.Add(new TemplateCodeDeleteStep());
            pipeline.Steps.Add(new SolutionRenameStep());
            pipeline.Steps.Add(new CreateProjectResultZipStep());

            return pipeline;
        }
    }
}
