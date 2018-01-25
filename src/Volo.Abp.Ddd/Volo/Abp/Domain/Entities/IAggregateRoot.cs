namespace Volo.Abp.Domain.Entities
{
    /// <summary>
    /// Defines an aggregate root. It's primary key may not be "Id" or it may have a composite primary key.
    /// Use <see cref="IAggregateRoot{TPrimaryKey}"/> where possible for better integration to repositories and other structures in the framework.
    /// </summary>
    public interface IAggregateRoot : IEntity
    {

    }

    /// <summary>
    /// Defines an aggregate root with a single primary key with "Id" property.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    public interface IAggregateRoot<TPrimaryKey> : IAggregateRoot, IEntity<TPrimaryKey>
    {

    }
}
