using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed
{
    public interface IDistributedEventBus
    {
        /// <summary>
        /// Subscribes to an event.
        /// Given action is called for all event occurrences.
        /// </summary>
        /// <param name="action">Action to handle events</param>
        /// <typeparam name="TEvent">Event type</typeparam>
        IDisposable Subscribe<TEvent>(Func<TEvent, Task> action)
            where TEvent : class;

        /// <summary>
        /// Subscribes to an event. 
        /// Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="handler">Object to handle the event</param>
        IDisposable Subscribe<TEvent>(IEventHandler<TEvent> handler)
            where TEvent : class;

        /// <summary>
        /// Subscribes to an event.
        /// A new instance of <see cref="THandler"/> object is created for every event occurrence.
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <typeparam name="THandler">Type of the event handler</typeparam>
        IDisposable Subscribe<TEvent, THandler>()
            where TEvent : class
            where THandler : IEventHandler, new();

        /// <summary>
        /// Subscribes to an event.
        /// Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Object to handle the event</param>
        IDisposable Subscribe(Type eventType, IEventHandler handler);

        /// <summary>
        /// Subscribes to an event.
        /// Given factory is used to create/release handlers
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="factory">A factory to create/release handlers</param>
        IDisposable Subscribe<TEvent>(IEventHandlerFactory factory)
            where TEvent : class;

        /// <summary>
        /// Subscribes to an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="factory">A factory to create/release handlers</param>
        IDisposable Subscribe(Type eventType, IEventHandlerFactory factory);

        Task PublishAsync<TEvent>(TEvent eventData)
            where TEvent : class;

        Task PublishAsync(Type eventType, object eventData);
    }
}
