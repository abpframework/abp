using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Studio.Solution;

namespace Volo.Abp.Studio.ModuleInstalling.Steps
{
    public class AddToSolutionFileStep : ModuleInstallingPipelineStep
    {
        public override async Task ExecuteAsync(ModuleInstallingContext context)
        {
            var _solutionFileModuleAdder = context.ServiceProvider.GetRequiredService<ISolutionFileModuleAdder>();

            await _solutionFileModuleAdder.AddAsync(context.TargetModule, context.ModuleName);
        }
    }
}
