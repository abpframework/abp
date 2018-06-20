using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="T:Volo.Abp.Auditing.ICreationAudited" /> for an entity.
    /// </summary>
    [Serializable]
    public abstract class CreationAuditedEntity : Entity, ICreationAudited
    {
        /// <inheritdoc />
        public virtual DateTime CreationTime { get; set; }

        /// <inheritdoc />
        public virtual Guid? CreatorId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited"/> for an entity.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAudited
    {
        /// <inheritdoc />
        public virtual DateTime CreationTime { get; set; }

        /// <inheritdoc />
        public virtual Guid? CreatorId { get; set; }
    }
}