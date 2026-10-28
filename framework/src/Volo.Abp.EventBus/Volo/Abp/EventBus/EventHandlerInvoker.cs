using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus;

public class EventHandlerInvoker : IEventHandlerInvoker, ISingletonDependency
{
    // Keyed by the handler type itself, since different types can share a full name (such as proxies from different
    // generators), and weakly, so a collectible handler type can still be unloaded.
    private readonly ConditionalWeakTable<Type, ConcurrentDictionary<Type, EventHandlerInvokerCacheItem>> _cache;

    public EventHandlerInvoker()
    {
        _cache = new ConditionalWeakTable<Type, ConcurrentDictionary<Type, EventHandlerInvokerCacheItem>>();
    }

    public async Task InvokeAsync(IEventHandler eventHandler, object eventData, Type eventType)
    {
        var handlerType = eventHandler.GetType();
        var handlerCache = _cache.GetValue(handlerType, static _ => new ConcurrentDictionary<Type, EventHandlerInvokerCacheItem>());
        if (!handlerCache.TryGetValue(eventType, out var cacheItem))
        {
            cacheItem = CreateCacheItem(eventHandler, eventType);

            // A handler that cannot handle the event fails below; it is not cached, so it keeps no event type alive.
            if (cacheItem.Local != null || cacheItem.Distributed != null)
            {
                cacheItem = handlerCache.GetOrAdd(eventType, cacheItem);
            }
        }

        if (cacheItem.Local != null)
        {
            await cacheItem.Local.ExecutorAsync(eventHandler, eventData);
        }

        if (cacheItem.Distributed != null)
        {
            await cacheItem.Distributed.ExecutorAsync(eventHandler, eventData);
        }

        if (cacheItem.Local == null && cacheItem.Distributed == null)
        {
            throw new AbpException("The object instance is not an event handler. Object type: " + eventHandler.GetType().AssemblyQualifiedName);
        }
    }

    private static EventHandlerInvokerCacheItem CreateCacheItem(IEventHandler eventHandler, Type eventType)
    {
        var item = new EventHandlerInvokerCacheItem();

        // Checked on the instance: a handler can provide its interfaces at run time (IDynamicInterfaceCastable).
        if (typeof(ILocalEventHandler<>).MakeGenericType(eventType).IsInstanceOfType(eventHandler))
        {
            item.Local = (IEventHandlerMethodExecutor?)Activator.CreateInstance(typeof(LocalEventHandlerMethodExecutor<>).MakeGenericType(eventType));
        }

        if (typeof(IDistributedEventHandler<>).MakeGenericType(eventType).IsInstanceOfType(eventHandler))
        {
            item.Distributed = (IEventHandlerMethodExecutor?)Activator.CreateInstance(typeof(DistributedEventHandlerMethodExecutor<>).MakeGenericType(eventType));
        }

        return item;
    }
}
