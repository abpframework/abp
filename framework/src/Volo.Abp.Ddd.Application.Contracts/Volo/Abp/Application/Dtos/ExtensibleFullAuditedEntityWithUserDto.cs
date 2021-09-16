using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="IFullAuditedObject{TUser}"/> interface.
    /// It has the <see cref="Creator"/>, <see cref="LastModifier"/> and <see cref="Deleter"/> objects as a DTOs represent the related user.
    /// It also implements the <see cref="IHasExtraProperties"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    /// <typeparam name="TUserDto">Type of the User</typeparam>
    [Serializable]
    public abstract class ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey, TUserDto> : ExtensibleFullAuditedEntityDto<TPrimaryKey>, IFullAuditedObject<TUserDto>
    {
        /// <inheritdoc />
        public TUserDto Creator { get; set; }

        /// <inheritdoc />
        public TUserDto LastModifier { get; set; }

        /// <inheritdoc />
        public TUserDto Deleter { get; set; }
    }

    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="IFullAuditedObject{TUser}"/> interface.
    /// It has the <see cref="Creator"/>, <see cref="LastModifier"/> and <see cref="Deleter"/> objects as a DTOs represent the related user.
    /// It also implements the <see cref="IHasExtraProperties"/> interface.
    /// </summary>
    /// <typeparam name="TUserDto">Type of the User</typeparam>
    [Serializable]
    public abstract class ExtensibleFullAuditedEntityWithUserDto<TUserDto> : ExtensibleFullAuditedEntityDto,
        IFullAuditedObject<TUserDto>
    {
        /// <inheritdoc />
        public TUserDto Creator { get; set; }

        /// <inheritdoc />
        public TUserDto LastModifier { get; set; }

        /// <inheritdoc />
        public TUserDto Deleter { get; set; }
    }
}