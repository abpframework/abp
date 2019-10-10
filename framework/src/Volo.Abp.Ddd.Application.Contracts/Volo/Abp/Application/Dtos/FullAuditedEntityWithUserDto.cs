using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="IFullAuditedObjectObject{TUser}"/> interface.
    /// </summary>
    /// <typeparam name="TUserDto">Type of the User</typeparam>
    [Serializable]
    public abstract class FullAuditedEntityWithUserDto<TUserDto> : FullAuditedEntityDto, IFullAuditedObject<TUserDto>
    {
        /// <inheritdoc />
        public TUserDto Creator { get; set; }

        /// <inheritdoc />
        public TUserDto LastModifier { get; set; }

        /// <inheritdoc />
        public TUserDto Deleter { get; set; }
    }

    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="IFullAuditedObjectObject{TUser}"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    /// <typeparam name="TUserDto">Type of the User</typeparam>
    [Serializable]
    public abstract class FullAuditedEntityWithUserDto<TPrimaryKey, TUserDto> : FullAuditedEntityDto<TPrimaryKey>, IFullAuditedObject<TUserDto>
    {
        /// <inheritdoc />
        public TUserDto Creator { get; set; }
        
        /// <inheritdoc />
        public TUserDto LastModifier { get; set; }
        
        /// <inheritdoc />
        public TUserDto Deleter { get; set; }
    }
}