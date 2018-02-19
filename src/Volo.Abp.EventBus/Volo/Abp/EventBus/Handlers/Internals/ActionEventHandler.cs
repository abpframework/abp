using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Handlers.Internals
{
    /// <summary>
    /// This event handler is an adapter to be able to use an action as <see cref="IEventHandler{TEventData}"/> implementation.
    /// </summary>
    /// <typeparam name="TEventData">Event type</typeparam>
    internal class ActionEventHandler<TEventData> :
        IEventHandler<TEventData>,
        ITransientDependency
    {
        /// <summary>
        /// Action to handle the event.
        /// </summary>
        public Action<TEventData> Action { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ActionEventHandler{TEventData}"/>.
        /// </summary>
        /// <param name="handler">Action to handle the event</param>
        public ActionEventHandler(Action<TEventData> handler)
        {
            Action = handler;
        }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="eventData"></param>
        public void HandleEvent(TEventData eventData)
        {
            Action(eventData);
        }
    }
}