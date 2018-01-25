using System;

namespace Volo.Abp.Domain.Entities
{
    //public abstract class AggregateRoot : AggregateRoot<Guid>, IAggregateRoot
    //{

    //}

    public abstract class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {

    }
}