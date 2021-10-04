using Microsoft.Extensions.DependencyInjection;
using Rebus.Persistence.InMem;
using Rebus.Transport.InMem;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Rebus;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.DistributedEvents;

namespace DistDemoApp
{
    [DependsOn(
        typeof(AbpMongoDbModule),
        typeof(AbpEventBusRebusModule),
        typeof(DistDemoAppSharedModule)
    )]
    public class DistDemoAppMongoDbRebusModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpRebusEventBusOptions>(options =>
            {
                options.InputQueueName = "eventbus";
                options.Configurer = rebusConfigurer =>
                {
                    rebusConfigurer.Transport(t => t.UseInMemoryTransport(new InMemNetwork(), "eventbus"));
                    rebusConfigurer.Subscriptions(s => s.StoreInMemory());
                };
            });
        }

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
