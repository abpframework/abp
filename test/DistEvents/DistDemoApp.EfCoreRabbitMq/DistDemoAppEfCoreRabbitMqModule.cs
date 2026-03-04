using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;

namespace DistDemoApp
{
#if DISTDEMO_USE_MONGODB
    [DependsOn(
        typeof(DistDemoAppMongoDbInfrastructureModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(DistDemoAppSharedModule)
    )]
#else
    [DependsOn(
        typeof(DistDemoAppEntityFrameworkCoreInfrastructureModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(DistDemoAppSharedModule)
    )]
#endif
    public class DistDemoAppEfCoreRabbitMqModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
#if DISTDEMO_USE_MONGODB
            context.ConfigureDistDemoMongoInfrastructure();
#else
            context.ConfigureDistDemoEntityFrameworkInfrastructure();
#endif
            
            Configure<AbpDistributedEventBusOptions>(options =>
            {
                // options.Outboxes.Configure(config =>
                // {
                //     config.UseDbContext<TodoDbContext>();
                // });
                //
                // options.Inboxes.Configure(config =>
                // {
                //     config.UseDbContext<TodoDbContext>();
                // });
            });

            Configure<AbpRabbitMqOptions>(options =>
            {
                options.Connections.Default.HostName = "localhost";
            });

            Configure<AbpRabbitMqEventBusOptions>(options =>
            {
                options.ConnectionName = "Default";
                options.ClientName = "DistDemoApp";
                options.ExchangeName = "DistDemo";
            });
            
        }
    }
}