using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus
{
    /// <summary>
    /// This event handler is an adapter to be able to use an action as <see cref="IEventHandler{TEventData}"/> implementation.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    internal class ActionEventHandler<TEvent> :
        IEventHandler<TEvent>,
        ITransientDependency
    {
        /// <summary>
        /// Action to handle the event.
        /// </summary>
        public Action<TEvent> Action { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ActionEventHandler{TEventData}"/>.
        /// </summary>
        /// <param name="handler">Action to handle the event</param>
        public ActionEventHandler(Action<TEvent> handler)
        {
            Action = handler;
        }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventData"></param>
        public void HandleEvent(TEvent eventData)
        {
            Action(eventData);
        }
    }
}