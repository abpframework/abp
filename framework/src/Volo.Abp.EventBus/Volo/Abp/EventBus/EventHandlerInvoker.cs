using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus;

public class EventHandlerInvoker : IEventHandlerInvoker, ISingletonDependency
{
    private const string EventHandlerMethodName = "HandleEventAsync";
    private readonly ConcurrentDictionary<Type, EventHandlerMethodExecutor> _executorCache;

    public EventHandlerInvoker()
    {
        _executorCache = new ConcurrentDictionary<Type, EventHandlerMethodExecutor>();
    }

    public Task InvokeAsync(IEventHandler eventHandler, object eventData)
    {
        var handleType = eventHandler.GetType();
        var executor = _executorCache.GetOrAdd(handleType, type => EventHandlerMethodExecutor.Create(type.GetMethod(EventHandlerMethodName), type.GetTypeInfo()));

        return executor.ExecuteAsync(eventHandler, new[] { eventData });
    }
}
