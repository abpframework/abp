namespace Volo.Abp.Domain.Entities
{
    /// <summary>
    /// Defines an aggregate root. It's primary key may not be "Id" or it may have a composite primary key.
    /// Use <see cref="IAggregateRoot{TKey}"/> where possible for better integration to repositories and other structures in the framework.
    /// </summary>
    public interface IAggregateRoot : IEntity
    {

    }

    /// <summary>
    /// Defines an aggregate root with a single primary key with "Id" property.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
    {

    }
}
