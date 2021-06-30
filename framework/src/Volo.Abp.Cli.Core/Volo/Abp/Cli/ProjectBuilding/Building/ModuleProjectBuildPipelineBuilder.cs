using Volo.Abp.Cli.ProjectBuilding.Building.Steps;
using Volo.Abp.Cli.ProjectBuilding.Templates;

namespace Volo.Abp.Cli.ProjectBuilding.Building
{
    public static class ModuleProjectBuildPipelineBuilder
    {
        public static ProjectBuildPipeline Build(ProjectBuildContext context)
        {
            var pipeline = new ProjectBuildPipeline(context);

            pipeline.Steps.Add(new FileEntryListReadStep());
            pipeline.Steps.Add(new ProjectReferenceReplaceStep());
            pipeline.Steps.Add(new ReplaceCommonPropsStep());
            pipeline.Steps.Add(new ReplaceConfigureAwaitPropsStep());
            pipeline.Steps.Add(new UpdateNuGetConfigStep("/NuGet.Config"));
            pipeline.Steps.Add(new CreateProjectResultZipStep());

            return pipeline;
        }
    }
}
