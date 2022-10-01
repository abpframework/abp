using System;

namespace Volo.Abp.Domain.Entities.Events.Distributed.ExternalEntitySynchronizers;

public class Book : Entity<Guid>, IHasRemoteModificationTime
{
    public virtual DateTime? RemoteLastModificationTime { get; protected set; }

    public virtual int Sold { get; set; }

    protected Book()
    {
    }

    public Book(Guid id, int sold) : base(id)
    {
        Sold = sold;
    }
}