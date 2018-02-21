using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus
{
    /// <summary>
    /// Defines interface of the event bus.
    /// </summary>
    public interface IEventBus
    {
        #region Register

        /// <summary>
        /// Registers to an event.
        /// Given action is called for all event occurrences.
        /// </summary>
        /// <param name="action">Action to handle events</param>
        /// <typeparam name="TEventData">Event type</typeparam>
        IDisposable Register<TEventData>(Action<TEventData> action) 
            where TEventData : class;

        /// <summary>
        /// Registers to an event.
        /// Given action is called for all event occurrences.
        /// </summary>
        /// <param name="action">Action to handle events</param>
        /// <typeparam name="TEventData">Event type</typeparam>
        IDisposable AsyncRegister<TEventData>(Func<TEventData, Task> action) 
            where TEventData : class;

        /// <summary>
        /// Registers to an event. 
        /// Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="handler">Object to handle the event</param>
        IDisposable Register<TEventData>(IEventHandler<TEventData> handler) 
            where TEventData : class;

        /// <summary>
        /// Registers to an event. 
        /// Same (given) instance of the async handler is used for all event occurrences.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="handler">Object to handle the event</param>
        IDisposable AsyncRegister<TEventData>(IAsyncEventHandler<TEventData> handler)
            where TEventData : class;

        /// <summary>
        /// Registers to an event.
        /// A new instance of <see cref="THandler"/> object is created for every event occurrence.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <typeparam name="THandler">Type of the event handler</typeparam>
        IDisposable Register<TEventData, THandler>()
            where TEventData : class 
            where THandler : IEventHandler, new();

        /// <summary>
        /// Registers to an event.
        /// Same (given) instance of the handler is used for all event occurrences.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Object to handle the event</param>
        IDisposable Register(Type eventType, IEventHandler handler);

        /// <summary>
        /// Registers to an event.
        /// Given factory is used to create/release handlers
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="factory">A factory to create/release handlers</param>
        IDisposable Register<TEventData>(IEventHandlerFactory factory) 
            where TEventData : class;

        /// <summary>
        /// Registers to an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="factory">A factory to create/release handlers</param>
        IDisposable Register(Type eventType, IEventHandlerFactory factory);

        #endregion

        #region Unregister

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="action"></param>
        void Unregister<TEventData>(Action<TEventData> action) 
            where TEventData : class;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="action"></param>
        void AsyncUnregister<TEventData>(Func<TEventData, Task> action)
            where TEventData : class;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="handler">Handler object that is registered before</param>
        void Unregister<TEventData>(IEventHandler<TEventData> handler)
            where TEventData : class;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="handler">Handler object that is registered before</param>
        void AsyncUnregister<TEventData>(IAsyncEventHandler<TEventData> handler) 
            where TEventData : class;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Handler object that is registered before</param>
        void Unregister(Type eventType, IEventHandler handler);

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="factory">Factory object that is registered before</param>
        void Unregister<TEventData>(IEventHandlerFactory factory) 
            where TEventData : class;

        /// <summary>
        /// Unregisters from an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="factory">Factory object that is registered before</param>
        void Unregister(Type eventType, IEventHandlerFactory factory);

        /// <summary>
        /// Unregisters all event handlers of given event type.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        void UnregisterAll<TEventData>() 
            where TEventData : class;

        /// <summary>
        /// Unregisters all event handlers of given event type.
        /// </summary>
        /// <param name="eventType">Event type</param>
        void UnregisterAll(Type eventType);

        #endregion

        #region Trigger

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="eventData">Related data for the event</param>
        void Trigger<TEventData>(TEventData eventData) 
            where TEventData : class;

        /// <summary>
        /// Triggers an event.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="eventData">Related data for the event</param>
        void Trigger(Type eventType, object eventData);

        /// <summary>
        /// Triggers an event asynchronously.
        /// </summary>
        /// <typeparam name="TEventData">Event type</typeparam>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task TriggerAsync<TEventData>(TEventData eventData) 
            where TEventData : class;

        /// <summary>
        /// Triggers an event asynchronously.
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="eventData">Related data for the event</param>
        /// <returns>The task to handle async operation</returns>
        Task TriggerAsync(Type eventType, object eventData);
        
        #endregion
    }
}