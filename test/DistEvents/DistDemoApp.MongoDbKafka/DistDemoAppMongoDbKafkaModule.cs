using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Kafka;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.DistributedEvents;

namespace DistDemoApp
{
    [DependsOn(
        typeof(AbpMongoDbModule),
        typeof(AbpEventBusKafkaModule),
        typeof(DistDemoAppSharedModule)
    )]
    public class DistDemoAppMongoDbKafkaModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<TodoMongoDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });

            Configure<AbpDistributedEventBusOptions>(options =>
            {
                options.Outboxes.Configure(config =>
                {
                    config.UseMongoDbContext<TodoMongoDbContext>();
                });

                options.Inboxes.Configure(config =>
                {
                    config.UseMongoDbContext<TodoMongoDbContext>();
                });
            });
        }
    }
}
