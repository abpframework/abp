using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Studio.ModuleInstalling;

namespace Volo.Abp.CmsKit
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IModuleInstallingPipelineBuilder))]
    public class BlobStoringDatabaseInstallerPipelineBuilder : ModuleInstallingPipelineBuilderBase, IModuleInstallingPipelineBuilder, ITransientDependency
    {
        public async Task<ModuleInstallingPipeline> BuildAsync(ModuleInstallingContext context)
        {
            context.AddEfCoreConfigurationMethodDeclaration(
                new EfCoreConfigurationMethodDeclaration(
                    "Volo.CmsKit.EntityFrameworkCore",
                    "ConfigureCmsKit"
                )
            );
            
            return GetBasePipeline(context);
        }
    }
}
