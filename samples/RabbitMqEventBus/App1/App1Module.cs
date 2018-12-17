using Volo.Abp.EventBus.Distributed.RabbitMq;
using Volo.Abp.Modularity;

namespace App2
{
    [DependsOn(
        typeof(AbpEventBusRabbitMqModule)
        )]
    public class App1Module : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<RabbitMqDistributedEventBusOptions>(options =>
            {
                options.ClientName = "TestApp1";
                options.ExchangeName = "TestMessages";
            });
        }
    }
}