using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing;

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObject{TCreator}"/>.
/// </summary>
/// <typeparam name="TUser">Type of the user</typeparam>
[Serializable]
public abstract class CreationAuditedEntityWithUser<TUser> : CreationAuditedEntity, ICreationAuditedObject<TUser>
{
    /// <inheritdoc />
    public virtual TUser Creator { get; protected set; }
}

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObjectObject{TCreator}"/>.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
/// <typeparam name="TUser">Type of the user</typeparam>
[Serializable]
public abstract class CreationAuditedEntityWithUser<TKey, TUser> : CreationAuditedEntity<TKey>, ICreationAuditedObject<TUser>
{
    /// <inheritdoc />
    public virtual TUser Creator { get; protected set; }

    protected CreationAuditedEntityWithUser()
    {

    }

    protected CreationAuditedEntityWithUser(TKey id)
        : base(id)
    {

    }
}
