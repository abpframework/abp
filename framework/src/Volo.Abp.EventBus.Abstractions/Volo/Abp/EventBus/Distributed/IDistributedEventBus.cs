using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed;

public interface IDistributedEventBus : IEventBus
{
    /// <summary>
    /// Registers to an event. 
    /// Same (given) instance of the handler is used for all event occurrences.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="handler">Object to handle the event</param>
    IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler)
        where TEvent : class;

    /// <summary>
    /// Triggers an event.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="eventData">Related data for the event</param>
    /// <param name="onUnitOfWorkComplete">True, to publish the event at the end of the current unit of work, if available</param>
    /// <param name="useOutbox">True, to use the outbox pattern for reliable event publishing</param>
    /// <returns>The task to handle async operation</returns>
    Task PublishAsync<TEvent>(
        TEvent eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true)
        where TEvent : class;

    /// <summary>
    /// Triggers an event.
    /// </summary>
    /// <param name="eventType">Event type</param>
    /// <param name="eventData">Related data for the event</param>
    /// <param name="onUnitOfWorkComplete">True, to publish the event at the end of the current unit of work, if available</param>
    /// <param name="useOutbox">True, to use the outbox pattern for reliable event publishing</param>
    /// <returns>The task to handle async operation</returns>
    Task PublishAsync(
        Type eventType,
        object eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true);

    /// <summary>
    /// Registers to an event by its string-based event name.
    /// Same (given) instance of the handler is used for all event occurrences.
    /// Wraps the handler as <see cref="IDistributedEventHandler{DynamicEventData}"/>.
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="handler">Object to handle the event</param>
    IDisposable Subscribe(string eventName, IDistributedEventHandler<DynamicEventData> handler);

    /// <summary>
    /// Triggers an event by its string-based event name.
    /// Used for dynamic (type-less) event publishing over distributed event bus.
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="eventData">Related data for the event</param>
    /// <param name="onUnitOfWorkComplete">True, to publish the event at the end of the current unit of work, if available</param>
    /// <param name="useOutbox">True, to use the outbox pattern for reliable event publishing</param>
    /// <returns>The task to handle async operation</returns>
    Task PublishAsync(
        string eventName,
        object eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true);
}
