using System;
using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// This type of event can be used to notify just after update of multiple Entities.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
[Serializable]
public class BulkEntityUpdatedEventData<TEntity> : BulkEntityChangedEventData<TEntity>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entities">The entities which are updated</param>
    public BulkEntityUpdatedEventData(List<TEntity> entities)
        : base(entities)
    {

    }
}