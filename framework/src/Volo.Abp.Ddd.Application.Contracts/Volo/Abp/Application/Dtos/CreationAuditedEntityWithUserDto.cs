using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject{TCreator}"/> interface.
    /// It also has the <see cref="Creator"/> object as a DTO represents the user.
    /// </summary>
    /// <typeparam name="TUserDto">Type of the User DTO</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntityWithUserDto<TUserDto> : CreationAuditedEntityDto, ICreationAuditedObject<TUserDto>
    {
        public TUserDto Creator { get; set; }
    }

    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject{TCreator}"/> interface.
    /// It also has the <see cref="Creator"/> object as a DTO represents the user.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    /// <typeparam name="TUserDto">Type of the User DTO</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> : CreationAuditedEntityDto<TPrimaryKey>, ICreationAuditedObject<TUserDto>
    {
        public TUserDto Creator { get; set; }
    }
}