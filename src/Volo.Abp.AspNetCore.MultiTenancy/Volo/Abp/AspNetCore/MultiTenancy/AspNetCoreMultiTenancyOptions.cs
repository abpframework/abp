namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class AspNetCoreMultiTenancyOptions
    {
        /// <summary>
        /// Default: "__tenant".
        /// </summary>
        public string TenantKey { get; set; }

        public AspNetCoreMultiTenancyOptions()
        {
            TenantKey = "__tenant";
        }
    }
}