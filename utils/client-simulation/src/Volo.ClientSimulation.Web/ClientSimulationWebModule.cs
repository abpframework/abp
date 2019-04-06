using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.Modularity;

namespace Volo.ClientSimulation
{
    [DependsOn(
        typeof(ClientSimulationModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule)
        )]
    public class ClientSimulationWebModule : AbpModule
    {

    }
}
