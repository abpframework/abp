using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObject{TCreator}"/> interface.
    /// </summary>
    /// <typeparam name="TUserDto">Type of the User DTO</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntityWithUserDto<TUserDto> : CreationAuditedEntityDto, ICreationAuditedObject<TUserDto>
    {
        public TUserDto Creator { get; set; }
    }

    /// <summary>
    /// This class can be inherited by DTO classes to implement <see cref="ICreationAuditedObjectObject{TCreator}"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    /// <typeparam name="TUserDto">Type of the User DTO</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto> : CreationAuditedEntityDto<TPrimaryKey>, ICreationAuditedObject<TUserDto>
    {
        public TUserDto Creator { get; set; }
    }
}