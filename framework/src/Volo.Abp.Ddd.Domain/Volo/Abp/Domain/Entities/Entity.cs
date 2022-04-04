using System;
using System.Collections.Generic;

namespace Volo.Abp.Domain.Entities;

/// <inheritdoc/>
[Serializable]
public abstract class Entity : IEntity
{
    protected Entity()
    {
        EntityHelper.TrySetTenantId(this);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Keys = {GetKeys().JoinAsString(", ")}";
    }

    public abstract object[] GetKeys();

    public bool EntityEquals(IEntity other)
    {
        return EntityHelper.EntityEquals(this, other);
    }
}

/// <inheritdoc cref="IEntity{TKey}" />
[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    /// <inheritdoc/>
    public virtual TKey Id { get; protected set; }

    protected Entity()
    {

    }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public override object[] GetKeys()
    {
        return new object[] { Id };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Id = {Id}";
    }
}
