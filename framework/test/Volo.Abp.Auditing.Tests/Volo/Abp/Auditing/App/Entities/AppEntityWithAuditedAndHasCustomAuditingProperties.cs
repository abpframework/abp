using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Auditing.App.Entities;

[Audited]
public class AppEntityWithAuditedAndHasCustomAuditingProperties : AggregateRoot<Guid>
{
    protected AppEntityWithAuditedAndHasCustomAuditingProperties()
    {
    }

    public AppEntityWithAuditedAndHasCustomAuditingProperties(Guid id)
        : base(id)
    {
    }

    public DateTime? CreationTime { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTime? LastModificationTime { get; set; }
    public Guid? LastModifierId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletionTime { get; set; }
    public Guid? DeleterId { get; set; }
}
