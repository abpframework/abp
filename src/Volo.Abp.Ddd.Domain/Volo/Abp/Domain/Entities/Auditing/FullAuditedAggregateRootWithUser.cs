using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// Implements <see cref="IFullAudited{TUser}"/> to be a base class for full-audited aggregate roots.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class FullAuditedAggregateRootWithUser<TUser> : FullAuditedAggregateRoot, IFullAudited<TUser>
        where TUser : IEntity<long>
    {
        /// <inheritdoc />
        public virtual TUser Deleter { get; set; }

        /// <inheritdoc />
        public TUser Creator { get; set; }

        /// <inheritdoc />
        public TUser LastModifier { get; set; }
    }

    /// <summary>
    /// Implements <see cref="IFullAudited{TUser}"/> to be a base class for full-audited aggregate roots.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class FullAuditedAggregateRootWithUser<TPrimaryKey, TUser> : FullAuditedAggregateRoot<TPrimaryKey>, IFullAudited<TUser>
        where TUser : IEntity<long>
    {
        /// <inheritdoc />
        public virtual TUser Deleter { get; set; }

        /// <inheritdoc />
        public TUser Creator { get; set; }

        /// <inheritdoc />
        public TUser LastModifier { get; set; }
    }
}