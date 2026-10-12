using System;
using Volo.Abp.EventBus;

namespace Volo.Abp.Gdpr;

[Serializable]
public class GdprUserDataRequestedEto : IEventDataMayHaveTenantId
{
    public Guid? TenantId { get; set; }

    public Guid UserId { get; set; }
    
    public Guid RequestId { get; set; }

    public bool IsMultiTenant(out Guid? tenantId)
    {
        tenantId = TenantId;
        return TenantId.HasValue;
    }
}
