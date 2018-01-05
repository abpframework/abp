namespace Volo.Abp.MultiTenancy
{
    public class ConfigurationTenantStoreOptions
    {
        public Tenant[] Tenants { get; set; }

        public ConfigurationTenantStoreOptions()
        {
            Tenants = new Tenant[0];
        }
    }
}