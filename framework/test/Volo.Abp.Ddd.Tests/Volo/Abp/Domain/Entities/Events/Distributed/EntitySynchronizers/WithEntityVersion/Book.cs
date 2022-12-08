using System;
using System.Text.Json.Serialization;
using Volo.Abp.Auditing;

namespace Volo.Abp.Domain.Entities.Events.Distributed.EntitySynchronizers.WithEntityVersion;

public class Book : Entity<Guid>, IHasEntityVersion
{
    public virtual int Sold { get; set; }

    [JsonInclude] // the memory DB repository requires this or a ctor arg
    public virtual int EntityVersion { get; protected set; }

    protected Book()
    {
    }

    public Book(Guid id, int sold) : base(id)
    {
        Sold = sold;
    }
}