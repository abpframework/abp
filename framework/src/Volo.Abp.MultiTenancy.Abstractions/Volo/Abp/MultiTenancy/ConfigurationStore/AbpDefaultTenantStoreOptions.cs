using System;

namespace Volo.Abp.MultiTenancy.ConfigurationStore;

public class AbpDefaultTenantStoreOptions
{
    public TenantConfiguration[] Tenants { get; set; }

    public AbpDefaultTenantStoreOptions()
    {
        Tenants = Array.Empty<TenantConfiguration>();
    }
}
