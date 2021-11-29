using System;

namespace Volo.Abp.Domain.Entities.Events
{
    /// <summary>
    /// This type of event can be used to notify just after creation of an Entity.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityCreatedEventData<TEntity> : EntityChangedEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">The entity which is created</param>
        public EntityCreatedEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}