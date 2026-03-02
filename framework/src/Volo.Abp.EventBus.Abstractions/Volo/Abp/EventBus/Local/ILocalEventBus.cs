using System;
using System.Collections.Generic;

namespace Volo.Abp.EventBus.Local;

/// <summary>
/// Defines interface of the event bus.
/// </summary>
public interface ILocalEventBus : IEventBus
{
    /// <summary>
    /// Registers to an event.
    /// Same (given) instance of the handler is used for all event occurrences.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="handler">Object to handle the event</param>
    IDisposable Subscribe<TEvent>(ILocalEventHandler<TEvent> handler)
        where TEvent : class;

    /// <summary>
    /// Registers to a named event.
    /// Same (given) instance of the handler is used for all event occurrences.
    /// One event name maps to exactly one type (1:1 constraint).
    /// </summary>
    /// <typeparam name="TPayload">Payload type the handler expects</typeparam>
    /// <param name="eventName">Name of the event</param>
    /// <param name="handler">Object to handle the event</param>
    IDisposable Subscribe<TPayload>(string eventName, ILocalEventHandler<TPayload> handler)
        where TPayload : class;

    /// <summary>
    /// Gets the list of event handler factories for the given event type.
    /// </summary>
    /// <param name="eventType">Event type</param>
    List<EventTypeWithEventHandlerFactories> GetEventHandlerFactories(Type eventType);

    /// <summary>
    /// Gets the list of event handler factories for the given event name and type.
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="eventType">Event type</param>
    List<EventTypeWithEventHandlerFactories> GetEventHandlerFactories(string eventName, Type eventType);
}
