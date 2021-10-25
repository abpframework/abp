using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Studio.ModuleInstalling;

namespace Volo.Abp.BackgroundJobs
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IModuleInstallingPipelineBuilder))]
    public class BackgroundJobsInstallerPipelineBuilder : ModuleInstallingPipelineBuilderBase, IModuleInstallingPipelineBuilder, ITransientDependency
    {
        public async Task<ModuleInstallingPipeline> BuildAsync(ModuleInstallingContext context)
        {
            context.AddEfCoreConfigurationMethodDeclaration(
                new EfCoreConfigurationMethodDeclaration(
                    "Volo.Abp.BackgroundJobs.EntityFrameworkCore",
                    "ConfigureBackgroundJobs"
                )
            );
            
            return GetBasePipeline(context);
        }
    }
}
