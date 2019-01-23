using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;

//TODO: Rename Volo.Abp.EventBus.Distributed.RabbitMq namespace to Volo.Abp.EventBus.RabbitMq
namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    [DependsOn(
        typeof(AbpEventBusModule),
        typeof(AbpRabbitMqModule))]
    public class AbpEventBusRabbitMqModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<RabbitMqEventBusOptions>(configuration.GetSection("RabbitMqEventBus"));
        }
    }
}
