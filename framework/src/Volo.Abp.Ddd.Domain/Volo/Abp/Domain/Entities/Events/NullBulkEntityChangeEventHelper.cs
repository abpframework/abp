using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// Null-object implementation of <see cref="IBulkEntityChangeEventHelper"/>.
/// </summary>
public class NullBulkEntityChangeEventHelper : IBulkEntityChangeEventHelper
{
    /// <summary>
    /// Gets single instance of <see cref="NullBulkEntityChangeEventHelper"/> class.
    /// </summary>
    public static NullBulkEntityChangeEventHelper Instance { get; } = new NullBulkEntityChangeEventHelper();

    private NullBulkEntityChangeEventHelper()
    {
    }


    public void PublishBulkEntityCreatedEvent(List<object> entities)
    {
    }

    public void PublishBulkEntityUpdatedEvent(List<object> entity)
    {
    }

    public void PublishBulkEntityDeletedEvent(List<object> entity)
    {
    }
}