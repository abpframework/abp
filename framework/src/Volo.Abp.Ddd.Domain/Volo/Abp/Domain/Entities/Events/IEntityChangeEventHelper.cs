namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// Used to trigger entity change events.
/// </summary>
public interface IEntityChangeEventHelper
{
    void PublishEntityCreatedEvent(object entity);

    void PublishEntityUpdatedEvent(object entity);

    void PublishEntityDeletedEvent(object entity);
}
