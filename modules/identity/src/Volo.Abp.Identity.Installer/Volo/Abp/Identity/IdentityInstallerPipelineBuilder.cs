using Volo.Abp.DependencyInjection;
using Volo.Abp.Studio.ModuleInstalling;

namespace Volo.Abp.Identity
{
    public class IdentityInstallerPipelineBuilder : ModuleInstallingPipelineBuilderBase, IModuleInstallingPipelineBuilder, ITransientDependency
    {
        public ModuleInstallingPipeline Build(ModuleInstallingContext context)
        {
            return GetBasePipeline(context);
        }
    }
}
