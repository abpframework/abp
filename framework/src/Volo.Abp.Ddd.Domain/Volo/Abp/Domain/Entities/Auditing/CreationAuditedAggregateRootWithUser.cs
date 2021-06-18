using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAuditedObjectObject{TCreator}"/> for aggregate roots.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class CreationAuditedAggregateRootWithUser<TUser> : CreationAuditedAggregateRoot, ICreationAuditedObject<TUser>
    {
        /// <inheritdoc />
        public virtual TUser Creator { get; protected set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAuditedObjectObject{TCreator}"/> for aggregate roots.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    [Serializable]
    public abstract class CreationAuditedAggregateRootWithUser<TKey, TUser> : CreationAuditedAggregateRoot<TKey>, ICreationAuditedObject<TUser>
    {
        /// <inheritdoc />
        public virtual TUser Creator { get; protected set; }

        protected CreationAuditedAggregateRootWithUser()
        {

        }

        protected CreationAuditedAggregateRootWithUser(TKey id)
            : base(id)
        {

        }
    }
}
