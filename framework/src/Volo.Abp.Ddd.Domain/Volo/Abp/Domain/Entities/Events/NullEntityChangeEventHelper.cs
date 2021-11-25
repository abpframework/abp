namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// Null-object implementation of <see cref="IEntityChangeEventHelper"/>.
/// </summary>
public class NullEntityChangeEventHelper : IEntityChangeEventHelper
{
    /// <summary>
    /// Gets single instance of <see cref="NullEntityChangeEventHelper"/> class.
    /// </summary>
    public static NullEntityChangeEventHelper Instance { get; } = new NullEntityChangeEventHelper();

    private NullEntityChangeEventHelper()
    {
    }

    public void PublishEntityCreatingEvent(object entity)
    {
    }

    public void PublishEntityCreatedEvent(object entity)
    {
    }

    public void PublishEntityUpdatingEvent(object entity)
    {
    }

    public void PublishEntityUpdatedEvent(object entity)
    {
    }

    public void PublishEntityDeletingEvent(object entity)
    {
    }

    public void PublishEntityDeletedEvent(object entity)
    {
    }
}
