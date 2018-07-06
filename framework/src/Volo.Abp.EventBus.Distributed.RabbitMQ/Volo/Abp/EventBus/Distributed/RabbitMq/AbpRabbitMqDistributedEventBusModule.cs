using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    [DependsOn(typeof(AbpDistributedEventBusModule))]
    public class AbpRabbitMqDistributedEventBusModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpRabbitMqDistributedEventBusModule>();
        }
    }
}
