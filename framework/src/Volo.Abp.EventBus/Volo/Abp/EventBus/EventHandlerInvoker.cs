using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus;

public class EventHandlerInvoker : IEventHandlerInvoker, ISingletonDependency
{
    private const string EventHandlerMethodName = "HandleEventAsync";
    private readonly ConcurrentDictionary<string, EventHandlerMethodExecutor> _executorCache;

    public EventHandlerInvoker()
    {
        _executorCache = new ConcurrentDictionary<string, EventHandlerMethodExecutor>();
    }

    public Task InvokeAsync(IEventHandler eventHandler, object eventData, Type eventType)
    {
        var handleType = eventHandler.GetType();
        var key = $"{handleType.FullName}_{eventType.FullName}";

        var executor = _executorCache.GetOrAdd(key, _ => EventHandlerMethodExecutor.Create(GetHandleEventMethodInfo(handleType, eventType), handleType.GetTypeInfo()));

        return executor.ExecuteAsync(eventHandler, new[] { eventData });
    }

    private static MethodInfo GetHandleEventMethodInfo(Type handleType, Type eventType)
    {
        var methods = handleType.GetMethods().Where(x => x.Name == EventHandlerMethodName).ToArray();
        return methods.Length == 1 ? methods.First() : methods.FirstOrDefault(x => x.GetParameters().Any(param => param.ParameterType == eventType));
    }
}
