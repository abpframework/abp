using System;
using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// This type of event can be used to notify just after deletion of multiple Entities.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
[Serializable]
public class BulkEntityDeletedEventData<TEntity> : BulkEntityChangedEventData<TEntity>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entity">The entities which are deleted</param>
    public BulkEntityDeletedEventData(List<TEntity> entity)
        : base(entity)
    {

    }
}