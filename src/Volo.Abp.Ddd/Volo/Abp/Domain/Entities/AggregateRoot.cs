namespace Volo.Abp.Domain.Entities
{

    /// <inheritdoc cref="IAggregateRoot" />
    public abstract class AggregateRoot : IAggregateRoot
    {

    }

    /// <inheritdoc cref="IAggregateRoot{TKey}" />
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
    {

    }
}