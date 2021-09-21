using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject{TCreator}"/> interface.
    /// It has the <see cref="Creator"/> object as a DTO represents the user.
    /// It also implements the <see cref="IHasExtraProperties"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    /// <typeparam name="TUserDto">Type of the User DTO</typeparam>
    [Serializable]
    public abstract class ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> : ExtensibleCreationAuditedEntityDto<TPrimaryKey>, ICreationAuditedObject<TUserDto>
    {
        public TUserDto Creator { get; set; }
    }

    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject{TCreator}"/> interface.
    /// It has the <see cref="Creator"/> object as a DTO represents the user.
    /// It also implements the <see cref="IHasExtraProperties"/> interface.
    /// </summary>
    /// <typeparam name="TUserDto">Type of the User DTO</typeparam>
    [Serializable]
    public abstract class ExtensibleCreationAuditedEntityWithUserDto<TUserDto> : ExtensibleCreationAuditedEntityDto,
        ICreationAuditedObject<TUserDto>
    {
        public TUserDto Creator { get; set; }
    }
}