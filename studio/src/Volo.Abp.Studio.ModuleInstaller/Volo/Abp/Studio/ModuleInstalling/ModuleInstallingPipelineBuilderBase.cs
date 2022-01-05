using System.Linq;
using Volo.Abp.Studio.ModuleInstalling.Steps;

namespace Volo.Abp.Studio.ModuleInstalling;

public abstract class ModuleInstallingPipelineBuilderBase
{
    protected ModuleInstallingPipeline GetBasePipeline(ModuleInstallingContext context)
    {
        var pipeline = new ModuleInstallingPipeline(context);

        if (context.WithSourceCode)
        {
            pipeline.Add(new SourceCodeDownloadStep());
            pipeline.Add(new AssemblyVersionStep());

            if (context.AddToSolutionFile)
            {
                pipeline.Add(new AddToSolutionFileStep());
            }
        }

        pipeline.Add(new PackageReferencingStep());

        if (context.EfCoreConfigurationMethodDeclarations.Any())
        {
            pipeline.Add(new AddEfCoreConfigurationMethodStep());
        }

        return pipeline;
    }
}
