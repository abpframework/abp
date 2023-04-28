using System;
using JetBrains.Annotations;

namespace Volo.Abp.Domain.Entities;

/// <summary>
/// Standard interface for an entity that MUST have an owner of type <typeparamref name="TOwner"/>.
/// </summary>
public interface IMustHaveOwner<TOwner> : IMustHaveOwner
{
    /// <summary>
    /// Reference to the owner.
    /// </summary>
    [CanBeNull]
    TOwner Owner { get; }
}

/// <summary>
/// Standard interface for an entity that MUST have an owner.
/// </summary>
public interface IMustHaveOwner
{
    Guid OwnerId { get; }
}