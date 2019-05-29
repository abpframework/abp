using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.ClientSimulation
{
    [DependsOn(
        typeof(ClientSimulationModule),
        typeof(AspNetCoreMvcUiThemeSharedModule)
        )]
    public class ClientSimulationWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ClientSimulationWebModule>("Volo.ClientSimulation");
            });
        }
    }
}
