using System.Collections.Generic;
using Volo.Abp.AspNetCore.MultiTenancy;

namespace Volo.Abp.MultiTenancy;

public static class AbpMultiTenancyOptionsExtensions
{
    public static void AddDomainTenantResolver(this AbpTenantResolveOptions options, string domainFormat)
    {
        options.TenantResolvers.InsertAfter(
            r => r is CurrentUserTenantResolveContributor,
            new DomainTenantResolveContributor(domainFormat)
        );
    }
}
