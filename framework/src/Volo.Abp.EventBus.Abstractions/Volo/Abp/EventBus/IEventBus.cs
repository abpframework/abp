using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus;

public interface IEventBus
{
    /// <summary>
    /// Triggers an event.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="eventData">Related data for the event</param>
    /// <param name="onUnitOfWorkComplete">True, to publish the event at the end of the current unit of work, if available</param>
    /// <returns>The task to handle async operation</returns>
    Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true)
        where TEvent : class;

    /// <summary>
    /// Triggers an event.
    /// </summary>
    /// <param name="eventType">Event type</param>
    /// <param name="eventData">Related data for the event</param>
    /// <param name="onUnitOfWorkComplete">True, to publish the event at the end of the current unit of work, if available</param>
    /// <returns>The task to handle async operation</returns>
    Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true);

    /// <summary>
    /// Registers to an event.
    /// Given action is called for all event occurrences.
    /// </summary>
    /// <param name="action">Action to handle events</param>
    /// <typeparam name="TEvent">Event type</typeparam>
    IDisposable Subscribe<TEvent>(Func<TEvent, Task> action)
        where TEvent : class;

    /// <summary>
    /// Registers to an event.
    /// A new instance of <typeparamref name="THandler"/> object is created for every event occurrence.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <typeparam name="THandler">Type of the event handler</typeparam>
    IDisposable Subscribe<TEvent, THandler>()
        where TEvent : class
        where THandler : IEventHandler, new();

    /// <summary>
    /// Registers to an event.
    /// Same (given) instance of the handler is used for all event occurrences.
    /// </summary>
    /// <param name="eventType">Event type</param>
    /// <param name="handler">Object to handle the event</param>
    IDisposable Subscribe(Type eventType, IEventHandler handler);

    /// <summary>
    /// Registers to an event.
    /// Given factory is used to create/release handlers
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="factory">A factory to create/release handlers</param>
    IDisposable Subscribe<TEvent>(IEventHandlerFactory factory)
        where TEvent : class;

    /// <summary>
    /// Registers to an event.
    /// </summary>
    /// <param name="eventType">Event type</param>
    /// <param name="factory">A factory to create/release handlers</param>
    IDisposable Subscribe(Type eventType, IEventHandlerFactory factory);

    /// <summary>
    /// Unregisters from an event.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="action"></param>
    void Unsubscribe<TEvent>(Func<TEvent, Task> action)
        where TEvent : class;

    /// <summary>
    /// Unregisters from an event.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="handler">Handler object that is registered before</param>
    void Unsubscribe<TEvent>(ILocalEventHandler<TEvent> handler)
        where TEvent : class;

    /// <summary>
    /// Unregisters from an event.
    /// </summary>
    /// <param name="eventType">Event type</param>
    /// <param name="handler">Handler object that is registered before</param>
    void Unsubscribe(Type eventType, IEventHandler handler);

    /// <summary>
    /// Unregisters from an event.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="factory">Factory object that is registered before</param>
    void Unsubscribe<TEvent>(IEventHandlerFactory factory)
        where TEvent : class;

    /// <summary>
    /// Unregisters from an event.
    /// </summary>
    /// <param name="eventType">Event type</param>
    /// <param name="factory">Factory object that is registered before</param>
    void Unsubscribe(Type eventType, IEventHandlerFactory factory);

    /// <summary>
    /// Unregisters all event handlers of given event type.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    void UnsubscribeAll<TEvent>()
        where TEvent : class;

    /// <summary>
    /// Unregisters all event handlers of given event type.
    /// </summary>
    /// <param name="eventType">Event type</param>
    void UnsubscribeAll(Type eventType);

    /// <summary>
    /// Triggers an event by name.
    /// The payload does not need to match the registered event type; it will be converted via JSON round-trip.
    /// If a type is registered for the given event name, existing typed handlers will be triggered.
    /// </summary>
    /// <param name="eventName">Name of the event (e.g. from <see cref="EventNameAttribute"/>)</param>
    /// <param name="eventData">Event payload (can be anonymous object, dictionary, etc.)</param>
    /// <param name="onUnitOfWorkComplete">True, to publish the event at the end of the current unit of work, if available</param>
    /// <returns>The task to handle async operation</returns>
    Task PublishByNameAsync(string eventName, object eventData, bool onUnitOfWorkComplete = true);

    /// <summary>
    /// Registers to an event by name.
    /// One event name maps to exactly one type (1:1 constraint).
    /// Attempting to register a different type for an already-mapped event name will throw.
    /// A single type may be registered under multiple event names (1:N).
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="payloadType">The type the handler expects to receive</param>
    /// <param name="factory">A factory to create/release handlers</param>
    IDisposable Subscribe(string eventName, Type payloadType, IEventHandlerFactory factory);

    /// <summary>
    /// Unregisters from a named event.
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="payloadType">The payload type used during subscription</param>
    /// <param name="factory">Factory object that is registered before</param>
    void Unsubscribe(string eventName, Type payloadType, IEventHandlerFactory factory);

    /// <summary>
    /// Unregisters all event handlers of given event name.
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    void UnsubscribeAll(string eventName);
}
