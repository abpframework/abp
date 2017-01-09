namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class AspNetCoreMultiTenancyOptions
    {
        /// <summary>
        /// Default: "__tenantId".
        /// </summary>
        public string TenantIdKey { get; set; }

        public AspNetCoreMultiTenancyOptions()
        {
            TenantIdKey = "__tenantId";
        }
    }
}