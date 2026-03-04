using Rebus.Transport.InMem;
using Volo.Abp.EventBus.Rebus;
using Volo.Abp.Modularity;

namespace DistDemoApp
{
#if DISTDEMO_USE_MONGODB
    [DependsOn(
        typeof(DistDemoAppMongoDbInfrastructureModule),
        typeof(AbpEventBusRebusModule),
        typeof(DistDemoAppSharedModule)
    )]
#else
    [DependsOn(
        typeof(DistDemoAppEntityFrameworkCoreInfrastructureModule),
        typeof(AbpEventBusRebusModule),
        typeof(DistDemoAppSharedModule)
    )]
#endif
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
                };
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
#if DISTDEMO_USE_MONGODB
            context.ConfigureDistDemoMongoInfrastructure();
#else
            context.ConfigureDistDemoEntityFrameworkInfrastructure();
#endif
        }
    }
}
