namespace Volo.Abp.EventBus.Handlers
{
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
