using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Pipeline;
using Rebus.Pipeline.Receive;
using Rebus.ServiceProvider;
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
        var rebusOptions = preActions.Configure();
        Configure<AbpRebusEventBusOptions>(options =>
        {
            preActions.Configure(options);
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

            rebusOptions.Configurer?.Invoke(configure);
            return configure;
        }, startAutomatically: false, key: rebusOptions.RebusInstanceName);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        context
            .ServiceProvider
            .GetRequiredService<RebusDistributedEventBus>()
            .Initialize();

        var rebusOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpRebusEventBusOptions>>().Value;
        context.ServiceProvider
            .GetRequiredService<IBusRegistry>()
            .StartBus(rebusOptions.RebusInstanceName);
    }
}
