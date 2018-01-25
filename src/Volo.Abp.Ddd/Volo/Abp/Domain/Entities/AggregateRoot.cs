namespace Volo.Abp.Domain.Entities
{

    /// <inheritdoc cref="IAggregateRoot" />
    public abstract class AggregateRoot : IAggregateRoot
    {

    }

    /// <inheritdoc cref="IAggregateRoot{TPrimaryKey}" />
    public abstract class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {

    }
}