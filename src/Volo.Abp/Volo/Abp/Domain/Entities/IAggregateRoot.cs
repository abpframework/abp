namespace Volo.Abp.Domain.Entities
{
    public interface IAggregateRoot : IAggregateRoot<string>, IEntity
    {

    }

    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>
    {

    }
}
