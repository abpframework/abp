namespace Volo.Abp.MultiTenancy.ConfigurationStore
{
    public class DefaultTenantStoreOptions
    {
        public TenantInfo[] Tenants { get; set; }

        public DefaultTenantStoreOptions()
        {
            Tenants = new TenantInfo[0];
        }
    }
}