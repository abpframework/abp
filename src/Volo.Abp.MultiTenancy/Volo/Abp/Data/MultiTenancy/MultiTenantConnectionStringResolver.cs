using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.DependencyInjection;

namespace Volo.Abp.Data.MultiTenancy
{
    //TODO: It would be better to use composition over inheritance on connection string resolve progress!
    //TODO: Create a replace service registration convention, instead of custom registration in AbpMultiTenancyModule?

    [DisableConventionalRegistration]
    public class MultiTenantConnectionStringResolver : DefaultConnectionStringResolver
    {
        private readonly IMultiTenancyManager _multiTenancyManager;
        private readonly ITenantConnectionStringStore _tenantConnectionStringStore;

        public MultiTenantConnectionStringResolver(
            IOptions<DbConnectionOptions> options,
            IMultiTenancyManager multiTenancyManager, 
            ITenantConnectionStringStore tenantConnectionStringStore)
            : base(options)
        {
            _multiTenancyManager = multiTenancyManager;
            _tenantConnectionStringStore = tenantConnectionStringStore;
        }

        public override string Resolve(string databaseName = null)
        {
            var tenant = _multiTenancyManager.CurrentTenant;
            if (tenant == null)
            {
                return base.Resolve(databaseName);
            }

            var connectionString = _tenantConnectionStringStore.GetConnectionStringOrNull(tenant.Id, databaseName);
            if (connectionString == null)
            {
                return base.Resolve(databaseName);
            }

            //TODO: If given tenant did not specified a databaseName specific connection string, then use the default connection string for databaseName, not tenant's default database

            return connectionString;
        }
    }
}
