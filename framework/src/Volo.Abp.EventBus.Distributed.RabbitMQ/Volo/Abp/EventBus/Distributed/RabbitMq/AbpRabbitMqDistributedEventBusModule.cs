using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    [DependsOn(typeof(AbpEventBusModule))]
    public class AbpRabbitMqDistributedEventBusModule : AbpModule
    {

    }
}
