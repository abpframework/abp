using System.Threading.Tasks;

namespace Volo.Abp.Studio.ModuleInstalling
{
    public interface IModuleInstallingPipelineBuilder
    {
        Task<ModuleInstallingPipeline> BuildAsync(ModuleInstallingContext context);
    }
}
