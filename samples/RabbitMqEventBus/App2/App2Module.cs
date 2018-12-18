using Volo.Abp.Autofac;
using Volo.Abp.EventBus.Distributed.RabbitMq;
using Volo.Abp.Modularity;

namespace App2
{
    [DependsOn(
        typeof(AbpEventBusRabbitMqModule),
        typeof(AbpAutofacModule)
        )]
    public class App2Module : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<RabbitMqDistributedEventBusOptions>(options =>
            {
                options.ClientName = "TestApp2";
                options.ExchangeName = "TestMessages";
            });
        }
    }
}