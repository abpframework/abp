using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Auditing;

/// <summary>
/// This class can be used to simplify implementing <see cref="T:Volo.Abp.Auditing.ICreationAuditedObject" /> for an entity.
/// </summary>
[Serializable]
public abstract class CreationAuditedEntity : Entity, ICreationAuditedObject
{
    /// <inheritdoc />
    public virtual DateTime CreationTime { get; protected set; }

    /// <inheritdoc />
    public virtual Guid? CreatorId { get; protected set; }
}

/// <summary>
/// This class can be used to simplify implementing <see cref="ICreationAuditedObject"/> for an entity.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
[Serializable]
public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAuditedObject
{
    /// <inheritdoc />
    public virtual DateTime CreationTime { get; protected set; }

    /// <inheritdoc />
    public virtual Guid? CreatorId { get; protected set; }

    protected CreationAuditedEntity()
    {

    }

    protected CreationAuditedEntity(TKey id)
        : base(id)
    {

    }
}
