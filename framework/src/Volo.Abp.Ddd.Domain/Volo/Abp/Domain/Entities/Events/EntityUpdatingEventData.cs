using System;

namespace Volo.Abp.Domain.Entities.Events
{
    /// <summary>
    /// This type of event is used to notify just before update of an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    [Obsolete("This event is no longer needed and identical to EntityUpdatedEventData. Please use EntityUpdatedEventData instead.")]
    public class EntityUpdatingEventData<TEntity> : EntityChangingEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">The entity which is being updated</param>
        public EntityUpdatingEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}