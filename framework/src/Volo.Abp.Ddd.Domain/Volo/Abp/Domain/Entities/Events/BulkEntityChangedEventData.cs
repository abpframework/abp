using System;
using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// Used to pass data for an event when multiple entities (<see cref="IEntity"/>) are changed (created, updated or deleted).
/// See <see cref="BulkEntityCreatedEventData{TEntity}"/>, <see cref="BulkEntityDeletedEventData{TEntity}"/> and <see cref="BulkEntityUpdatedEventData{TEntity}"/> classes.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
[Serializable]
public class BulkEntityChangedEventData<TEntity> : BulkEntityEventData<TEntity>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entities">Changed entities in this event</param>
    public BulkEntityChangedEventData(List<TEntity> entities)
        : base(entities)
    {

    }
}