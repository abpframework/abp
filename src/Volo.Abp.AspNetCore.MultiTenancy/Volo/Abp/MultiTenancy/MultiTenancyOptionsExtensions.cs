using Volo.Abp.AspNetCore.MultiTenancy;

namespace Volo.Abp.MultiTenancy
{
    public static class MultiTenancyOptionsExtensions
    {
        public static void AddDomainTenantResolver(this TenantResolveOptions options, string domainFormat)
        {
            options.TenantResolvers.Insert(0, new DomainTenantResolver(domainFormat));
        }
    }
}