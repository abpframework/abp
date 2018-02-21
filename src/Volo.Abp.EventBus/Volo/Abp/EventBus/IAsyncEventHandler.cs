using System.Threading.Tasks;

namespace Volo.Abp.EventBus
{
    /// <summary>
    /// Defines an interface of a class that handles events asynchrounously of type <see cref="IAsyncEventHandler{TEventData}"/>.
    /// </summary>
    /// <typeparam name="TEventData">Event type to handle</typeparam>
    public interface IAsyncEventHandler<in TEventData> : IEventHandler
    {
        /// <summary>
        /// Handler handles the event by implementing this method.
        /// </summary>
        /// <param name="eventData">Event data</param>
        Task HandleEventAsync(TEventData eventData);
    }
}
