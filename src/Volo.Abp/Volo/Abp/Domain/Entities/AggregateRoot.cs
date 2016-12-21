namespace Volo.Abp.Domain.Entities
{
    public class AggregateRoot : AggregateRoot<string>, IAggregateRoot
    {

    }

    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {

    }
}