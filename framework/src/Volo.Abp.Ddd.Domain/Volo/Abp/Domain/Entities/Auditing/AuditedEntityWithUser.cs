using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAuditedObject{TUser}"/>.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class AuditedEntityWithUser<TUser> : AuditedEntity, IAuditedObject<TUser>
        where TUser : IEntity<Guid>
    {
        /// <inheritdoc />
        public virtual TUser Creator { get; set; }

        /// <inheritdoc />
        public virtual TUser LastModifier { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAuditedObject{TUser}"/>.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class AuditedEntityWithUser<TKey, TUser> : AuditedEntity<TKey>, IAuditedObject<TUser>
        where TUser : IEntity<Guid>
    {
        /// <inheritdoc />
        public virtual TUser Creator { get; set; }

        /// <inheritdoc />
        public virtual TUser LastModifier { get; set; }

        protected AuditedEntityWithUser()
        {

        }

        protected AuditedEntityWithUser(TKey id)
            : base(id)
        {

        }
    }
}