using System;

namespace Volo.Abp.MultiTenancy;

[Serializable]
public class TenantChangedEvent
{
    public Guid? Id { get; set; }
    public string? NormalizedName { get; set; }

    public TenantChangedEvent(Guid? id = null, string? normalizedName = null)
    {
        Id = id;
        NormalizedName = normalizedName;
    }
}