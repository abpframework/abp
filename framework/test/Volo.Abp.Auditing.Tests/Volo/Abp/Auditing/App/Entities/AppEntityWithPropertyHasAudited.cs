using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Auditing.App.Entities;

public class AppEntityWithPropertyHasAudited : AggregateRoot<Guid>
{
    protected AppEntityWithPropertyHasAudited()
    {

    }

    public AppEntityWithPropertyHasAudited(Guid id, string name)
        : base(id)
    {
        Name = name;
    }

    [Audited]
    public string Name { get; set; }
}
