using Microsoft.Extensions.DependencyInjection;
using Rebus.Handlers;
using Rebus.ServiceProvider;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Rebus
{
    [DependsOn(
        typeof(AbpEventBusModule))]
    public class AbpEventBusRebusModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var options = context.Services.ExecutePreConfiguredActions<AbpRebusEventBusOptions>();

            context.Services.AddTransient(typeof(IHandleMessages<>), typeof(RebusDistributedEventHandlerAdapter<>));

            Configure<AbpRebusEventBusOptions>(rebusOptions =>
            {
                rebusOptions.Configurer = options.Configurer;
                rebusOptions.Publish = options.Publish;
                rebusOptions.InputQueueName = options.InputQueueName;
            });

            context.Services.AddRebus(configurer =>
            {
                options.Configurer?.Invoke(configurer);
                return configurer;
            });

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider.UseRebus();

            context
                .ServiceProvider
                .GetRequiredService<RebusDistributedEventBus>()
                .Initialize();
        }
    }
}
