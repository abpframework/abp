using Volo.DependencyInjection;

namespace Volo.Abp.Data.MultiTenancy
{
    public sealed class NullTenantConnectionStringStore : ITenantConnectionStringStore, ISingletonDependency
    {
        public string GetDefaultConnectionStringOrNull(string tenantId)
        {
            //No tenant specific connection string by default
            return null;
        }

        public string GetConnectionStringOrNull(string tenantId, string connStringName)
        {
            //No tenant specific connection string by default
            return null;
        }
    }
}