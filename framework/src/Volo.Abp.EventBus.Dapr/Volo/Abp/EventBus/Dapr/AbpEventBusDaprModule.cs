using Volo.Abp.Dapr;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Dapr;

[DependsOn(
    typeof(AbpEventBusModule),
    typeof(AbpDaprModule)
    )]
public class AbpEventBusDaprModule : AbpModule
{
}