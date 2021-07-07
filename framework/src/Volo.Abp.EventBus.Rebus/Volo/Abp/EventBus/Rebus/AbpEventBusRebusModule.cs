using Microsoft.Extensions.DependencyInjection;
using Rebus.Handlers;
using Rebus.Retry.Simple;
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
            var abpEventBusOptions = context.Services.ExecutePreConfiguredActions<AbpEventBusOptions>();
            var options = context.Services.ExecutePreConfiguredActions<AbpRebusEventBusOptions>();;

            context.Services.AddTransient(typeof(IHandleMessages<>), typeof(RebusDistributedEventHandlerAdapter<>));

            Configure<AbpRebusEventBusOptions>(rebusOptions =>
            {
                context.Services.ExecutePreConfiguredActions(rebusOptions);
            });

            context.Services.AddRebus(configure =>
            {
                if (abpEventBusOptions.RetryStrategyOptions != null)
                {
                    configure.Options(b =>
                        b.SimpleRetryStrategy(
                            errorQueueAddress: abpEventBusOptions.DeadLetterName ?? options.InputQueueName + "_dead_letter",
                            maxDeliveryAttempts: abpEventBusOptions.RetryStrategyOptions.MaxRetryAttempts));
                }

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
