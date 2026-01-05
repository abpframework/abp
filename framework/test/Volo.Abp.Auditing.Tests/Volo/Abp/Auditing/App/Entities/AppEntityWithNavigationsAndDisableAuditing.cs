using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Auditing.App.Entities;

[Audited]
public class AppEntityWithNavigationsAndDisableAuditing : AggregateRoot<Guid>
{
    protected AppEntityWithNavigationsAndDisableAuditing()
    {

    }

    public AppEntityWithNavigationsAndDisableAuditing(Guid id, string name)
        : base(id)
    {
        Name = name;
    }

    public string Name { get; set; }

    [DisableAuditing]
    public virtual List<AppEntityWithNavigationsAndDisableAuditingChildOneToMany> OneToMany { get; set; }
}


public class AppEntityWithNavigationsAndDisableAuditingChildOneToMany : Entity<Guid>
{
    public Guid AppEntityWithNavigationsAndDisableAuditingId { get; set; }

    public string ChildName { get; set; }
}
