using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AzureServiceBus;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Azure;

[DependsOn(
    typeof(AbpEventBusModule),
    typeof(AbpAzureServiceBusModule)
)]
public class AbpEventBusAzureModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<AbpAzureEventBusOptions>(configuration.GetSection("Azure:EventBus"));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        context
            .ServiceProvider
            .GetRequiredService<AzureDistributedEventBus>()
            .Initialize();
    }
}
