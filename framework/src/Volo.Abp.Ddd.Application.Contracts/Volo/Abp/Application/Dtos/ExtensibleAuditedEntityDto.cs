using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="IAuditedObject"/> interface.
    /// It also implements the <see cref="IHasExtraProperties"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    [Serializable]
    public abstract class ExtensibleAuditedEntityDto<TPrimaryKey> : ExtensibleCreationAuditedEntityDto<TPrimaryKey>, IAuditedObject
    {
        /// <inheritdoc />
        public DateTime? LastModificationTime { get; set; }

        /// <inheritdoc />
        public Guid? LastModifierId { get; set; }
    }

    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="IAuditedObject"/> interface.
    /// It also implements the <see cref="IHasExtraProperties"/> interface.
    /// </summary>
    [Serializable]
    public abstract class ExtensibleAuditedEntityDto : ExtensibleCreationAuditedEntityDto, IAuditedObject
    {
        /// <inheritdoc />
        public DateTime? LastModificationTime { get; set; }

        /// <inheritdoc />
        public Guid? LastModifierId { get; set; }
    }
}