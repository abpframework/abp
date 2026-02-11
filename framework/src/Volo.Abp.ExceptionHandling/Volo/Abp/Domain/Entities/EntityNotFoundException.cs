using System;

namespace Volo.Abp.Domain.Entities;

/// <summary>
/// This exception is thrown if an entity is expected to be found but not found.
/// </summary>
public class EntityNotFoundException<TEntityType> : EntityNotFoundException
{
    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException{TEntityType}"/> object.
    /// </summary>
    public EntityNotFoundException()
        : base(typeof(TEntityType))
    {
    }
    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException{TEntityType}"/> object.
    /// </summary>
    public EntityNotFoundException(object? id)
        : base(typeof(TEntityType), id)
    {
    }
    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException{TEntityType}"/> object.
    /// </summary>
    public EntityNotFoundException(object? id, Exception? innerException)
        : base(typeof(TEntityType), id, innerException)
    {
    }
}

/// <summary>
/// This exception is thrown if an entity is expected to be found but not found.
/// </summary>
public class EntityNotFoundException : AbpException
{
    /// <summary>
    /// Type of the entity.
    /// </summary>
    public Type? EntityType { get; set; }

    /// <summary>
    /// Id of the Entity.
    /// </summary>
    public object? Id { get; set; }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException()
    {

    }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException(Type entityType)
        : this(entityType, null, null)
    {

    }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException(Type entityType, object? id)
        : this(entityType, id, null)
    {

    }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException(Type entityType, object? id, Exception? innerException)
        : base(
            id == null
                ? $"There is no such an entity given id. Entity type: {entityType.FullName}"
                : $"There is no such an entity. Entity type: {entityType.FullName}, id: {id}",
            innerException)
    {
        EntityType = entityType;
        Id = id;
    }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    public EntityNotFoundException(string message)
        : base(message)
    {

    }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
