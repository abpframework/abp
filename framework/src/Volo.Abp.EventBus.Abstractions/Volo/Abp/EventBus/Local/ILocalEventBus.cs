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
    /// Gets the list of event handler factories for the given event type.
    /// </summary>
    /// <param name="eventType">Event type</param>
    /// <returns></returns>
     List<EventTypeWithEventHandlerFactories> GetEventHandlerFactories(Type eventType);
    
    /// <summary>
    /// Gets the list of event handler factories for the given string-based event name.
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <returns>List of event handler factories registered for the given event name</returns>
    List<EventTypeWithEventHandlerFactories> GetDynamicEventHandlerFactories(string eventName);
}
