using System;

namespace Volo.Abp.EventBus;

/// <summary>
/// Used to unregister a <see cref="IEventHandlerFactory"/> on <see cref="Dispose"/> method.
/// </summary>
public class EventHandlerFactoryUnregistrar : IDisposable
{
    private readonly IEventBus _eventBus;
    private readonly Type _eventType;
    private readonly IEventHandlerFactory _factory;

    public EventHandlerFactoryUnregistrar(IEventBus eventBus, Type eventType, IEventHandlerFactory factory)
    {
        _eventBus = eventBus;
        _eventType = eventType;
        _factory = factory;
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe(_eventType, _factory);
    }
}

/// <summary>
/// Used to unregister an <see cref="IEventHandlerFactory"/> for a string-based event name on <see cref="IDisposable.Dispose"/> method.
/// </summary>
public class DynamicEventHandlerFactoryUnregistrar : IDisposable
{
    private readonly IEventBus _eventBus;
    private readonly string _eventName;
    private readonly IEventHandlerFactory _factory;

    public DynamicEventHandlerFactoryUnregistrar(IEventBus eventBus, string eventName, IEventHandlerFactory factory)
    {
        _eventBus = eventBus;
        _eventName = eventName;
        _factory = factory;
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe(_eventName, _factory);
    }
}
