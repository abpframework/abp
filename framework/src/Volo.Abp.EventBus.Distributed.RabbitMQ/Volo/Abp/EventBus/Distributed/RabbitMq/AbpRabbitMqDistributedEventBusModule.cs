using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    [DependsOn(typeof(AbpDistributedEventBusModule))]
    public class AbpRabbitMqDistributedEventBusModule : AbpModule
    {

    }
}
