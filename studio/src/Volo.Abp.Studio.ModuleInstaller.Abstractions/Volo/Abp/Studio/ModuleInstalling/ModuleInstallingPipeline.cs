using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Studio.ModuleInstalling
{
    public class ModuleInstallingPipeline
    {
        public ModuleInstallingContext Context { get; }

        public List<ModuleInstallingPipelineStep> Steps { get; }

        public ModuleInstallingPipeline(ModuleInstallingContext context)
        {
            Context = context;
            Steps = new List<ModuleInstallingPipelineStep>();
        }

        public async Task ExecuteAsync()
        {
            foreach (var step in Steps)
            {
                await step.ExecuteAsync(Context);
            }
        }
    }
}
