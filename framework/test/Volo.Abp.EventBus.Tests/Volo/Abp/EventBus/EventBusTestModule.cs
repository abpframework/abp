using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus
{
    [DependsOn(typeof(EventBusModule))]
    public class EventBusTestModule : AbpModule
    {

    }
}