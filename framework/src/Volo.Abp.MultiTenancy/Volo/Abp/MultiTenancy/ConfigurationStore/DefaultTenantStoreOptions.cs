namespace Volo.Abp.MultiTenancy.ConfigurationStore
{
    public class DefaultTenantStoreOptions
    {
        public TenantConfiguration[] Tenants { get; set; }

        public DefaultTenantStoreOptions()
        {
            Tenants = new TenantConfiguration[0];
        }
    }
}