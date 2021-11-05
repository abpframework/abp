using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject"/> interface.
    /// It also implements the <see cref="IHasExtraProperties"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    [Serializable]
    public abstract class ExtensibleCreationAuditedEntityDto<TPrimaryKey> : ExtensibleEntityDto<TPrimaryKey>, ICreationAuditedObject
    {
        /// <inheritdoc />
        public DateTime CreationTime { get; set; }

        /// <inheritdoc />
        public Guid? CreatorId { get; set; }
    }

    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject"/> interface.
    /// It also implements the <see cref="IHasExtraProperties"/> interface.
    /// </summary>
    [Serializable]
    public abstract class ExtensibleCreationAuditedEntityDto : ExtensibleEntityDto, ICreationAuditedObject
    {
        /// <inheritdoc />
        public DateTime CreationTime { get; set; }

        /// <inheritdoc />
        public Guid? CreatorId { get; set; }
    }
}