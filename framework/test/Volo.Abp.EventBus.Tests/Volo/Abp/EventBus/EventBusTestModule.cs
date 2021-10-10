using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class EventBusTestModule : AbpModule
    {
    }
}
