using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.EventBus.RabbitMq;

[DependsOn(
    typeof(AbpEventBusModule),
    typeof(AbpRabbitMqModule))]
public class AbpEventBusRabbitMqModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        Configure<AbpRabbitMqEventBusOptions>(configuration.GetSection("RabbitMQ:EventBus"));
        context.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<AbpRabbitMqEventBusOptions>, PostConfigureAbpRabbitMqEventBusOptions>());
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        context
            .ServiceProvider
            .GetRequiredService<IRabbitMqDistributedEventBus>()
            .Initialize();
    }
}
