using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="ICreationAudited"/> interface.
    /// </summary>
    [Serializable]
    public abstract class CreationAuditedEntityDto : EntityDto, ICreationAudited
    {
        /// <inheritdoc />
        public DateTime CreationTime { get; set; }

        /// <inheritdoc />
        public Guid? CreatorUserId { get; set; }
    }

    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="ICreationAudited"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>, ICreationAudited
    {
        /// <inheritdoc />
        public DateTime CreationTime { get; set; }

        /// <inheritdoc />
        public Guid? CreatorUserId { get; set; }
    }
}