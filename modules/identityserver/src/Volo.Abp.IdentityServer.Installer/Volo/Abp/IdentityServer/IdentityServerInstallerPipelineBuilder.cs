using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Studio.ModuleInstalling;

namespace Volo.Abp.IdentityServer;

[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
[ExposeServices(typeof(IModuleInstallingPipelineBuilder))]
public class IdentityServerInstallerPipelineBuilder : ModuleInstallingPipelineBuilderBase, IModuleInstallingPipelineBuilder, ITransientDependency
{
    public async Task<ModuleInstallingPipeline> BuildAsync(ModuleInstallingContext context)
    {
        context.AddEfCoreConfigurationMethodDeclaration(
            new EfCoreConfigurationMethodDeclaration(
                "Volo.Abp.IdentityServer.EntityFrameworkCore",
                "ConfigureIdentityServer"
            )
        );

        return GetBasePipeline(context);
    }
}
