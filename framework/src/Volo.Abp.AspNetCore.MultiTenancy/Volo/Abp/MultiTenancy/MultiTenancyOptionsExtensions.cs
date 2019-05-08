using System.Collections.Generic;
using Volo.Abp.AspNetCore.MultiTenancy;

namespace Volo.Abp.MultiTenancy
{
    public static class MultiTenancyOptionsExtensions
    {
        public static void AddDomainTenantResolver(this TenantResolveOptions options, string domainFormat)
        {
            options.TenantResolvers.InsertAfter(
                r => r is CurrentUserTenantResolveContributor,
                new DomainTenantResolveContributor(domainFormat)
            );
        }
    }
}