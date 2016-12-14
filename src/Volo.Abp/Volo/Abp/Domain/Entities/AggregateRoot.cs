namespace Volo.Abp.Domain.Entities
{
    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {

    }
}