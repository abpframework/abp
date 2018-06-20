using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited"/> for aggregate roots.
    /// </summary>
    [Serializable]
    public abstract class AuditedAggregateRoot : CreationAuditedAggregateRoot, IAudited
    {
        /// <inheritdoc />
        public virtual DateTime? LastModificationTime { get; set; }

        /// <inheritdoc />
        public virtual Guid? LastModifierId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited"/> for aggregate roots.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class AuditedAggregateRoot<TKey> : CreationAuditedAggregateRoot<TKey>, IAudited
    {
        /// <inheritdoc />
        public virtual DateTime? LastModificationTime { get; set; }

        /// <inheritdoc />
        public virtual Guid? LastModifierId { get; set; }
    }
}