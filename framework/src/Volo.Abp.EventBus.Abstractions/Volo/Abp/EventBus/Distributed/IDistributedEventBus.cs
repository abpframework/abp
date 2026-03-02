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

    Task PublishAsync<TEvent>(
        TEvent eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true)
        where TEvent : class;

    Task PublishAsync(
        Type eventType,
        object eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true);

    /// <summary>
    /// Triggers a distributed event by name.
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="eventData">Event payload (can be anonymous object, dictionary, etc.)</param>
    /// <param name="onUnitOfWorkComplete">True, to publish the event at the end of the current unit of work, if available</param>
    /// <param name="useOutbox">True, to use the outbox pattern</param>
    Task PublishByNameAsync(
        string eventName,
        object eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true);

    /// <summary>
    /// Registers to a named distributed event.
    /// Same (given) instance of the handler is used for all event occurrences.
    /// One event name maps to exactly one type (1:1 constraint).
    /// </summary>
    /// <typeparam name="TPayload">Payload type the handler expects</typeparam>
    /// <param name="eventName">Name of the event</param>
    /// <param name="handler">Object to handle the event</param>
    IDisposable Subscribe<TPayload>(string eventName, IDistributedEventHandler<TPayload> handler)
        where TPayload : class;
}
