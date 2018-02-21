namespace Volo.Abp.EventBus
{
    /// <summary>
    /// Undirect base interface for all event handlers.
    /// Implement <see cref="IEventHandler{TEventData}"/> instead of this one.
    /// </summary>
    public interface IEventHandler
    {
        
    }

    /// <summary>
    /// Defines an interface of a class that handles events of type <see cref="IEventHandler{TEventData}"/>.
    /// </summary>
    /// <typeparam name="TEventData">Event type to handle</typeparam>
    public interface IEventHandler<in TEventData> : IEventHandler
    {
        /// <summary>
        /// Handler handles the event by implementing this method.
        /// </summary>
        /// <param name="eventData">Event data</param>
        void HandleEvent(TEventData eventData);
    }
}