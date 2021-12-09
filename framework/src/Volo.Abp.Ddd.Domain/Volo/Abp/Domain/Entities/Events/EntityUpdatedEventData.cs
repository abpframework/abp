using System;

namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// This type of event can be used to notify just after update of an Entity.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
[Serializable]
public class EntityUpdatedEventData<TEntity> : EntityChangedEventData<TEntity>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entity">The entity which is updated</param>
    public EntityUpdatedEventData(TEntity entity)
        : base(entity)
    {

    }
}
