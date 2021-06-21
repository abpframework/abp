using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Domain.Entities.Events
{
    /// <summary>
    /// Used to pass data for an event that is related to with an <see cref="IEntity"/> object.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityEventData<TEntity> : IEventDataWithInheritableGenericArgument, IEventDataMayHaveTenantId
    {
        /// <summary>
        /// Related entity with this event.
        /// </summary>
        public TEntity Entity { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">Related entity with this event</param>
        public EntityEventData(TEntity entity)
        {
            Entity = entity;
        }

        public virtual object[] GetConstructorArgs()
        {
            return new object[] { Entity };
        }

        public virtual bool IsMultiTenant(out Guid? tenantId)
        {
            if (Entity is IMultiTenant multiTenantEntity)
            {
                tenantId = multiTenantEntity.TenantId;
                return true;
            }

            tenantId = null;
            return false;
        }
    }
}
