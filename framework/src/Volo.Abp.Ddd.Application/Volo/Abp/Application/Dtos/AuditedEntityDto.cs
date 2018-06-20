using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="IAudited"/> interface.
    /// </summary>
    [Serializable]
    public abstract class AuditedEntityDto : CreationAuditedEntityDto, IAudited
    {
        /// <inheritdoc />
        public DateTime? LastModificationTime { get; set; }

        /// <inheritdoc />
        public Guid? LastModifierId { get; set; }
    }

    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="IAudited"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    [Serializable]
    public abstract class AuditedEntityDto<TPrimaryKey> : CreationAuditedEntityDto<TPrimaryKey>, IAudited
    {
        /// <inheritdoc />
        public DateTime? LastModificationTime { get; set; }

        /// <inheritdoc />
        public Guid? LastModifierId { get; set; }
    }
}