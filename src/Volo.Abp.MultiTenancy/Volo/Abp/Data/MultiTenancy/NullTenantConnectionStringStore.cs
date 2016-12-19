using Volo.DependencyInjection;

namespace Volo.Abp.Data.MultiTenancy
{
    public sealed class NullTenantConnectionStringStore : ITenantConnectionStringStore, ISingletonDependency
    {
        public string GetConnectionStringOrNull(string tenantId, string databaseName)
        {
            //No tenant specific connection string by default
            return null;
        }
    }
}