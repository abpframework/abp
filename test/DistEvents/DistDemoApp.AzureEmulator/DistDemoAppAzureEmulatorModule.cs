using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus.Azure;
using Volo.Abp.Modularity;
using Volo.Abp.AzureServiceBus;

namespace DistDemoApp;

#if DISTDEMO_USE_MONGODB
[DependsOn(
    typeof(DistDemoAppMongoDbInfrastructureModule),
    typeof(AbpEventBusAzureModule),
    typeof(DistDemoAppSharedModule)
)]
#else
[DependsOn(
    typeof(DistDemoAppEntityFrameworkCoreInfrastructureModule),
    typeof(AbpEventBusAzureModule),
    typeof(DistDemoAppSharedModule)
)]
#endif
public class DistDemoAppAzureEmulatorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IPublisherPool, EmulatorPublisherPool>();
        context.Services.AddSingleton<IProcessorPool, EmulatorProcessorPool>();
#if DISTDEMO_USE_MONGODB
        context.ConfigureDistDemoMongoInfrastructure();
#else
        context.ConfigureDistDemoEntityFrameworkInfrastructure();
#endif

        Configure<AbpAzureServiceBusOptions>(options =>
        {
            options.Connections.Default.ConnectionString =
                "Endpoint=sb://localhost:5673;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
            options.Connections.Default.Processor = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false
            };
        });

        Configure<AbpAzureEventBusOptions>(options =>
        {
            options.ConnectionName = "Default";
            options.SubscriberName = "DistDemoAzureSubscriber";
            options.TopicName = "DistDemoAzureTopic";
        });
    }
}
