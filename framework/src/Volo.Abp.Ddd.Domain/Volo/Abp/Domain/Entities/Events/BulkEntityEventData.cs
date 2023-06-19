using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// Used to pass data for an event that is related to multiple <see cref="IEntity"/> objects.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
[Serializable]
public class BulkEntityEventData<TEntity> : IEventDataWithInheritableGenericArgument
{
    /// <summary>
    /// Related entities with this event.
    /// </summary>
    public List<TEntity> Entities { get; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entities">Related entities with this event</param>
    public BulkEntityEventData(List<TEntity> entities)
    {
        Entities = entities;
    }

    public virtual object[] GetConstructorArgs()
    {
        return new object[] { Entities };
    }
}