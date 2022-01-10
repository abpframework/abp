using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus;

public class EventHandlerInvoker : IEventHandlerInvoker, ISingletonDependency
{
    private readonly ConcurrentDictionary<string, IEventHandlerMethodExecutor> _cache;

    public EventHandlerInvoker()
    {
        _cache = new ConcurrentDictionary<string, IEventHandlerMethodExecutor>();
    }

    public async Task InvokeAsync(IEventHandler eventHandler, object eventData, Type eventType)
    {
        var notAnEventHandler = true;
        if (typeof(ILocalEventHandler<>).MakeGenericType(eventType).IsInstanceOfType(eventHandler))
        {
            var eventHandlerCall = _cache.GetOrAdd($"{typeof(LocalEventHandlerMethodExecutor<>).FullName}{eventHandler.GetType().FullName}-{eventType.FullName}",
                (_) => (IEventHandlerMethodExecutor)Activator.CreateInstance(typeof(LocalEventHandlerMethodExecutor<>).MakeGenericType(eventType)));
            await eventHandlerCall.ExecutorAsync(eventHandler, eventData);
            notAnEventHandler = false;
        }

        if (typeof(IDistributedEventHandler<>).MakeGenericType(eventType).IsInstanceOfType(eventHandler))
        {
            var eventHandlerCall = _cache.GetOrAdd($"{typeof(DistributedEventHandlerMethodExecutor<>).FullName}{eventHandler.GetType().FullName}-{eventType.FullName}",
                (_) => (IEventHandlerMethodExecutor)Activator.CreateInstance(typeof(DistributedEventHandlerMethodExecutor<>).MakeGenericType(eventType)));
            await eventHandlerCall.ExecutorAsync(eventHandler, eventData);
            notAnEventHandler = false;
        }

        if (notAnEventHandler)
        {
            throw new AbpException("The object instance is not an event handler. Object type: " + eventHandler.GetType().AssemblyQualifiedName);
        }
    }
}
