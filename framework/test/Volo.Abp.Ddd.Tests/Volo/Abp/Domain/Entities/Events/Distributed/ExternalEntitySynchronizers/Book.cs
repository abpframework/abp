using System;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Events.Distributed.ExternalEntitySynchronizers;

public class Book : Entity<Guid>, IHasEntityVersion
{
    public virtual int Sold { get; set; }

    public virtual int EntityVersion { get; protected set; }

    protected Book()
    {
    }

    public Book(Guid id, int sold, int entityVersion) : base(id)
    {
        Sold = sold;
        EntityVersion = entityVersion;
    }
}