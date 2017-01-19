using System;

namespace Volo.Abp.Domain.Entities
{
    public class AggregateRoot : AggregateRoot<Guid>, IAggregateRoot
    {

    }

    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {

    }
}