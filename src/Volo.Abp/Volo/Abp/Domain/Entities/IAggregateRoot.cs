namespace Volo.Abp.Domain.Entities
{
    //TODO: Domain events

    public interface IAggregateRoot : IAggregateRoot<string>, IEntity
    {

    }

    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>
    {

    }
}
