using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy;

public class AbpTenantResolveOptions
{
    [NotNull]
    public List<ITenantResolveContributor> TenantResolvers { get; }

    public AbpTenantResolveOptions()
    {
        TenantResolvers = new List<ITenantResolveContributor>
            {
                new CurrentUserTenantResolveContributor()
            };
    }
}
