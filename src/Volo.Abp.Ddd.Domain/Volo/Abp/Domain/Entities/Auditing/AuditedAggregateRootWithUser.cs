using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited{TUser}"/> for aggregate roots.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class AuditedAggregateRootWithUser<TUser> : AuditedAggregateRoot, IAudited<TUser>
        where TUser : IEntity<long>
    {
        /// <inheritdoc />
        public virtual TUser CreatorUser { get; set; }

        /// <inheritdoc />
        public virtual TUser LastModifierUser { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited{TUser}"/> for aggregate roots.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class AuditedAggregateRootWithUser<TPrimaryKey, TUser> : AuditedAggregateRoot<TPrimaryKey>, IAudited<TUser>
        where TUser : IEntity<long>
    {
        /// <inheritdoc />
        public virtual TUser CreatorUser { get; set; }

        /// <inheritdoc />
        public virtual TUser LastModifierUser { get; set; }
    }
}