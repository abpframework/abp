using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class AspNetCoreMultiTenancyOptions
    {
        /// <summary>
        /// Default: <see cref="TenantResolverConsts.DefaultTenantKey"/>.
        /// </summary>
        public string TenantKey { get; set; }

        public AspNetCoreMultiTenancyOptions()
        {
            TenantKey = TenantResolverConsts.DefaultTenantKey;
        }
    }
}