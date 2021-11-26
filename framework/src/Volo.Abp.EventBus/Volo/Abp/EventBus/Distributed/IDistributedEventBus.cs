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
}
