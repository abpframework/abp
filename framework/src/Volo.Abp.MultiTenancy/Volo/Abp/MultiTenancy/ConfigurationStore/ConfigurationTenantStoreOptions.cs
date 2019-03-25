namespace Volo.Abp.MultiTenancy.ConfigurationStore
{
    public class ConfigurationTenantStoreOptions
    {
        public TenantInfo[] Tenants { get; set; }

        public ConfigurationTenantStoreOptions()
        {
            Tenants = new TenantInfo[0];
        }
    }
}