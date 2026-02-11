using System;
using System.Collections.Generic;

namespace Volo.Abp.EventBus;

public class EventTypeWithEventHandlerFactories
{
    public Type EventType { get; }

    public List<IEventHandlerFactory> EventHandlerFactories { get; }

    public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
    {
        EventType = eventType;
        EventHandlerFactories = eventHandlerFactories;
    }
}
