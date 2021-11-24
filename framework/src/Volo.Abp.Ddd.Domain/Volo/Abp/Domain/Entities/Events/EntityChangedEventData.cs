using System;

namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// Used to pass data for an event when an entity (<see cref="IEntity"/>) is changed (created, updated or deleted).
/// See <see cref="EntityCreatedEventData{TEntity}"/>, <see cref="EntityDeletedEventData{TEntity}"/> and <see cref="EntityUpdatedEventData{TEntity}"/> classes.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
[Serializable]
public class EntityChangedEventData<TEntity> : EntityEventData<TEntity>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="entity">Changed entity in this event</param>
    public EntityChangedEventData(TEntity entity)
        : base(entity)
    {

    }
}
