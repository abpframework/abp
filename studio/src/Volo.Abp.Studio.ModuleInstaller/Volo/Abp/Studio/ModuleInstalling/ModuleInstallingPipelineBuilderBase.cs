using Volo.Abp.Studio.ModuleInstalling.Steps;

namespace Volo.Abp.Studio.ModuleInstalling
{
    public abstract class ModuleInstallingPipelineBuilderBase
    {
        protected ModuleInstallingPipeline GetBasePipeline(ModuleInstallingContext context)
        {
            var pipeline = new ModuleInstallingPipeline(context);

            if (context.WithSourceCode)
            {
                pipeline.Add(new SourceCodeDownloadStep());
            }

            pipeline.Add(new PackageReferencingStep());

            return pipeline;
        }
    }
}
