using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited{TUser}"/> for aggregate roots.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class CreationAuditedAggregateRootWithUser<TUser> : CreationAuditedAggregateRoot, ICreationAudited<TUser>
    {
        /// <inheritdoc />
        public virtual TUser Creator { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited{TUser}"/> for aggregate roots.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class CreationAuditedAggregateRootWithUser<TKey, TUser> : CreationAuditedAggregateRoot<TKey>, ICreationAudited<TUser>
    {
        /// <inheritdoc />
        public virtual TUser Creator { get; set; }
    }
}