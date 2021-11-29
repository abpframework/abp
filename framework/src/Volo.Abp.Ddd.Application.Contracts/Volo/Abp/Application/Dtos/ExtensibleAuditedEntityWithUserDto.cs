using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Application.Dtos;

/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="IAuditedObject"/> interface.
/// It has the <see cref="Creator"/> and <see cref="LastModifier"/> objects as a DTOs represent the related user.
/// It also implements the <see cref="IHasExtraProperties"/> interface.
/// </summary>
/// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
/// <typeparam name="TUserDto">Type of the User DTO</typeparam>
[Serializable]
public abstract class ExtensibleAuditedEntityWithUserDto<TPrimaryKey, TUserDto> : ExtensibleAuditedEntityDto<TPrimaryKey>, IAuditedObject<TUserDto>
{
    /// <inheritdoc />
    public TUserDto Creator { get; set; }

    /// <inheritdoc />
    public TUserDto LastModifier { get; set; }
}

/// <summary>
/// This class can be inherited by DTO classes to implement <see cref="IAuditedObject"/> interface.
/// It has the <see cref="Creator"/> and <see cref="LastModifier"/> objects as a DTOs represent the related user.
/// It also implements the <see cref="IHasExtraProperties"/> interface.
/// </summary>
/// <typeparam name="TUserDto">Type of the User DTO</typeparam>
[Serializable]
public abstract class ExtensibleAuditedEntityWithUserDto<TUserDto> : ExtensibleAuditedEntityDto,
    IAuditedObject<TUserDto>
{
    /// <inheritdoc />
    public TUserDto Creator { get; set; }

    /// <inheritdoc />
    public TUserDto LastModifier { get; set; }
}
