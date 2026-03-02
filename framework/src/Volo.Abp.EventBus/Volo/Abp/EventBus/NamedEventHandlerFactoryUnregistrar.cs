using System;

namespace Volo.Abp.EventBus;

public class NamedEventHandlerFactoryUnregistrar : IDisposable
{
    private readonly IEventBus _eventBus;
    private readonly string _eventName;
    private readonly Type _payloadType;
    private readonly IEventHandlerFactory _factory;

    public NamedEventHandlerFactoryUnregistrar(
        IEventBus eventBus,
        string eventName,
        Type payloadType,
        IEventHandlerFactory factory)
    {
        _eventBus = eventBus;
        _eventName = eventName;
        _payloadType = payloadType;
        _factory = factory;
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe(_eventName, _payloadType, _factory);
    }
}
