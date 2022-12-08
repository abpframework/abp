using System;

namespace Volo.Abp.Domain.Entities.Events.Distributed.EntitySynchronizers.WithoutEntityVersion;

public class Author : Entity<Guid>
{
    public virtual string Name { get; set; }

    protected Author()
    {
    }

    public Author(Guid id, string name) : base(id)
    {
        Name = name;
    }
}