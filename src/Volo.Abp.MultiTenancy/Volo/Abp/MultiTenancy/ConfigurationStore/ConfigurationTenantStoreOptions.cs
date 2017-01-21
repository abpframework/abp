namespace Volo.Abp.MultiTenancy.ConfigurationStore
{
    public class ConfigurationTenantStoreOptions
    {
        public TenantInformation[] Tenants { get; set; }

        public ConfigurationTenantStoreOptions()
        {
            Tenants = new TenantInformation[0];
        }
    }
}