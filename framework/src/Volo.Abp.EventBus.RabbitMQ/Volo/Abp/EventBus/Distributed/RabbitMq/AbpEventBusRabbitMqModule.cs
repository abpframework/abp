using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    [DependsOn(
        typeof(AbpEventBusModule),
        typeof(AbpRabbitMqModule))]
    public class AbpEventBusRabbitMqModule : AbpModule
    {

    }
}
