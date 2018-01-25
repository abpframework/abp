namespace Volo.Abp.Domain.Entities
{
    /// <summary>
    /// Defines an entity. It's primary key may not be "Id" or it mah have a composite primary key.
    /// Use <see cref="IEntity{TPrimaryKey}"/> where possible for better integration to repositories and other structures in the framework.
    /// </summary>
    public interface IEntity
    {

    }

    /// <summary>
    /// Defines an entity with a single primary key with "Id" property.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public interface IEntity<TPrimaryKey> : IEntity
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}
