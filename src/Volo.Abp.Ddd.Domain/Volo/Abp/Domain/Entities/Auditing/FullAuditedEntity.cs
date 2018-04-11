using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// Implements <see cref="IFullAudited"/> to be a base class for full-audited entities.
    /// </summary>
    [Serializable]
    public abstract class FullAuditedEntity : AuditedEntity, IFullAudited
    {
        /// <inheritdoc />
        public virtual bool IsDeleted { get; set; }

        /// <inheritdoc />
        public virtual Guid? DeleterId { get; set; }

        /// <inheritdoc />
        public virtual DateTime? DeletionTime { get; set; }
    }

    /// <summary>
    /// Implements <see cref="IFullAudited"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IFullAudited
    {
        /// <inheritdoc />
        public virtual bool IsDeleted { get; set; }

        /// <inheritdoc />
        public virtual Guid? DeleterId { get; set; }

        /// <inheritdoc />
        public virtual DateTime? DeletionTime { get; set; }
    }
}