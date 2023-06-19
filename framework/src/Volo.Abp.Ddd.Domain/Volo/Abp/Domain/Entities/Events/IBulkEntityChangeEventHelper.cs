using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// Used to trigger entity change events.
/// </summary>
public interface IBulkEntityChangeEventHelper
{
    void PublishBulkEntityCreatedEvent(List<object> entities);

    void PublishBulkEntityUpdatedEvent(List<object> entity);

    void PublishBulkEntityDeletedEvent(List<object> entity);
}