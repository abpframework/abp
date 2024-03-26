using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus;

[DependsOn(typeof(AbpEventBusModule))]
public class EventBusTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.TryAddTransient(typeof(MyGenericDistributedEventHandler<>));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var distributedEventBus = context.ServiceProvider.GetRequiredService<IDistributedEventBus>();
        var scopeFactory = context.ServiceProvider.GetRequiredService<IServiceScopeFactory>();

        var eventType = typeof(MySimpleEventData);
        var myHandlerType = typeof(MyGenericDistributedEventHandler<>).MakeGenericType(eventType);
        distributedEventBus.Subscribe(eventType, new IocEventHandlerFactory(scopeFactory, myHandlerType));
    }
}
