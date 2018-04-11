using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// Implements <see cref="IFullAudited{TUser}"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class FullAuditedEntityWithUser<TUser> : FullAuditedEntity, IFullAudited<TUser>
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
    /// Implements <see cref="IFullAudited{TUser}"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class FullAuditedEntityWithUser<TKey, TUser> : FullAuditedEntity<TKey>, IFullAudited<TUser>
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