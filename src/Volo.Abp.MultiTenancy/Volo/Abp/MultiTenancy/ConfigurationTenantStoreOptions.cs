namespace Volo.Abp.MultiTenancy
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