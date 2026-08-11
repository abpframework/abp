using System;
using Volo.Abp.EventBus;

namespace Volo.Abp.Gdpr;

[Serializable]
public class GdprUserDataPreparedEto : IEventDataMayHaveTenantId
{
    public Guid? TenantId { get; set; }

    public Guid RequestId { get; set; }

    public string Provider { get; set; } = default!;
    
    public GdprDataInfo Data { get; set; } = default!;

    public bool IsMultiTenant(out Guid? tenantId)
    {
        tenantId = TenantId;
        return TenantId.HasValue;
    }
}
