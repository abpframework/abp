using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Distributed
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class AbpDistributedEventBusModule : AbpModule
    {

    }
}
