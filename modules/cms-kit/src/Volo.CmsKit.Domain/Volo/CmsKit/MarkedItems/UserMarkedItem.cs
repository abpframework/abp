using System;
using Volo.Abp;
using Volo.Abp.Auditing;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.MarkedItems;

public class UserMarkedItem : BasicAggregateRoot<Guid>, IHasCreationTime, IMustHaveCreator, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid CreatorId { get; set; }

    public virtual DateTime CreationTime { get; set; }

    public string EntityId { get; protected set; }
    public string EntityType { get; protected set; }

    protected UserMarkedItem() { }

    internal UserMarkedItem(
        Guid id,
        [NotNull] string entityType,
        [NotNull] string entityId,
        Guid creatorId,
        Guid? tenantId = null)
        : base(id)
    {
        EntityType = Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        EntityId = Check.NotNullOrWhiteSpace(entityId, nameof(entityId));
        CreatorId = creatorId;
        TenantId = tenantId;
    }
}
