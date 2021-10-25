using System.Threading.Tasks;

namespace Volo.Abp.Studio.ModuleInstalling
{
    public abstract class ModuleInstallingPipelineStep
    {
        public abstract Task ExecuteAsync(ModuleInstallingContext context);
    }
}
