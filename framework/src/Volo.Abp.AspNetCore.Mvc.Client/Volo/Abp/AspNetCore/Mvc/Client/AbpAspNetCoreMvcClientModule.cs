using Volo.Abp.EventBus;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Client
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcClientCommonModule),
        typeof(AbpEventBusModule)
        )]
    public class AbpAspNetCoreMvcClientModule : AbpModule
    {
    }
}
