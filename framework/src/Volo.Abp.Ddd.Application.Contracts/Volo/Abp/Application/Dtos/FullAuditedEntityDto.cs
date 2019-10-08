using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="IFullAuditedObject"/> interface.
    /// </summary>
    [Serializable]
    public abstract class FullAuditedEntityDto : AuditedEntityDto, IFullAuditedObject
    {
        /// <inheritdoc />
        public bool IsDeleted { get; set; }

        /// <inheritdoc />
        public Guid? DeleterId { get; set; }

        /// <inheritdoc />
        public DateTime? DeletionTime { get; set; }
    }

    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="IFullAuditedObject"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    [Serializable]
    public abstract class FullAuditedEntityDto<TPrimaryKey> : AuditedEntityDto<TPrimaryKey>, IFullAuditedObject
    {
        /// <inheritdoc />
        public bool IsDeleted { get; set; }

        /// <inheritdoc />
        public Guid? DeleterId { get; set; }

        /// <inheritdoc />
        public DateTime? DeletionTime { get; set; }
    }
}