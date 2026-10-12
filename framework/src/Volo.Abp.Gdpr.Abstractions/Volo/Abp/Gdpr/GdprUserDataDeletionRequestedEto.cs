using System;
using Volo.Abp.EventBus;

namespace Volo.Abp.Gdpr;

[Serializable]
public class GdprUserDataDeletionRequestedEto : IEventDataMayHaveTenantId
{
    public Guid? TenantId { get; set; }

    public Guid UserId { get; set; }

    public bool IsMultiTenant(out Guid? tenantId)
    {
        tenantId = TenantId;
        return TenantId.HasValue;
    }
}
