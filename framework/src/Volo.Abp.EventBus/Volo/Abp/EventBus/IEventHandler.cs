using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EventBus;

/// <summary>
/// Undirect base interface for all event handlers.
/// Implement <see cref="ILocalEventHandler{TEvent}"/> or <see cref="IDistributedEventHandler{TEvent}"/> instead of this one.
/// </summary>
public interface IEventHandler
{

}
