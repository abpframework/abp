using System;

namespace Volo.Abp.Domain.Entities.Events
{
    /// <summary>
    /// This type of event is used to notify just before deletion of an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityDeletingEventData<TEntity> : EntityChangingEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">The entity which is being deleted</param>
        public EntityDeletingEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}