using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.EventBus.RabbitMq
{
    [DependsOn(
        typeof(EventBusModule),
        typeof(RabbitMqModule))]
    public class EventBusRabbitMqModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<RabbitMqEventBusOptions>(configuration.GetSection("RabbitMQ:EventBus"));
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context
                .ServiceProvider
                .GetRequiredService<RabbitMqDistributedEventBus>()
                .Initialize();
        }
    }
}
