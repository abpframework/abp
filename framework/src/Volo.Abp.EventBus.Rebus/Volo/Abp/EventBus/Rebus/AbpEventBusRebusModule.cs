using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Pipeline;
using Rebus.Pipeline.Receive;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Rebus;

[DependsOn(
    typeof(AbpEventBusModule))]
public class AbpEventBusRebusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(IHandleMessages<>), typeof(RebusDistributedEventHandlerAdapter<>));

        var preActions = context.Services.GetPreConfigureActions<AbpRebusEventBusOptions>();
        Configure<AbpRebusEventBusOptions>(rebusOptions =>
        {
            preActions.Configure(rebusOptions);
        });

        context.Services.AddRebus(configure =>
        {
            configure.Options(options =>
            {
                options.Decorate<IPipeline>(d =>
                {
                    var step = new AbpRebusEventHandlerStep();
                    var pipeline = d.Get<IPipeline>();

                    return new PipelineStepInjector(pipeline).OnReceive(step, PipelineRelativePosition.After, typeof(ActivateHandlersStep));
                });
            });

            preActions.Configure().Configurer?.Invoke(configure);
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
