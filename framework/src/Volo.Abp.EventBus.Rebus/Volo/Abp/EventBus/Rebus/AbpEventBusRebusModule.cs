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
            var options = context.Services.ExecutePreConfiguredActions<AbpRebusEventBusOptions>();;

            context.Services.AddTransient(typeof(IHandleMessages<>), typeof(RebusDistributedEventHandlerAdapter<>));

            Configure<AbpRebusEventBusOptions>(rebusOptions =>
            {
                context.Services.ExecutePreConfiguredActions(rebusOptions);
            });

            context.Services.AddRebus(configure =>
            {
                options.Configurer?.Invoke(configure);
                return configure;
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
