using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy;

public class AbpTenantResolveOptions
{
    [NotNull]
    public List<ITenantResolveContributor> TenantResolvers { get; }

    /// <summary>
    /// Fallback tenant to use when no other resolver resolves a tenant.
    /// </summary>
    public string? FallbackTenant { get; set; }

    public AbpTenantResolveOptions()
    {
        TenantResolvers = new List<ITenantResolveContributor>();
    }
}
