using Volo.Abp.EventBus.Kafka;
using Volo.Abp.Kafka;
using Volo.Abp.Modularity;

namespace DistDemoApp
{
#if DISTDEMO_USE_MONGODB
    [DependsOn(
        typeof(DistDemoAppMongoDbInfrastructureModule),
        typeof(AbpEventBusKafkaModule),
        typeof(DistDemoAppSharedModule)
    )]
#else
    [DependsOn(
        typeof(DistDemoAppEntityFrameworkCoreInfrastructureModule),
        typeof(AbpEventBusKafkaModule),
        typeof(DistDemoAppSharedModule)
    )]
#endif
    public class DistDemoAppMongoDbKafkaModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
#if DISTDEMO_USE_MONGODB
            context.ConfigureDistDemoMongoInfrastructure();
#else
            context.ConfigureDistDemoEntityFrameworkInfrastructure();
#endif

            Configure<AbpKafkaOptions>(options =>
            {
                options.Connections.Default.BootstrapServers = "localhost:9092";
            });

            Configure<AbpKafkaEventBusOptions>(options =>
            {
                options.ConnectionName = "Default";
                options.TopicName = "DistDemoTopic";
                options.GroupId = "DistDemoApp";
            });
        }
    }
}
