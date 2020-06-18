using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class AbpAspNetCoreMultiTenancyOptions
    {
        /// <summary>
        /// Default: <see cref="TenantResolverConsts.DefaultTenantKey"/>.
        /// </summary>
        public string TenantKey { get; set; }

        public AbpAspNetCoreMultiTenancyOptions()
        {
            TenantKey = TenantResolverConsts.DefaultTenantKey;
        }
    }
}