namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// Used to trigger entity change events.
/// </summary>
public interface IEntityChangeEventHelper
{
    void PublishEntityCreatingEvent(object entity);
    void PublishEntityCreatedEvent(object entity);

    void PublishEntityUpdatingEvent(object entity);
    void PublishEntityUpdatedEvent(object entity);

    void PublishEntityDeletingEvent(object entity);
    void PublishEntityDeletedEvent(object entity);
}
