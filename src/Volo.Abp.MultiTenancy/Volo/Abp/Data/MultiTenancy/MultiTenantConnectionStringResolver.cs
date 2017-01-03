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
            IOptionsSnapshot<DbConnectionOptions> options,
            IMultiTenancyManager multiTenancyManager, 
            ITenantConnectionStringStore tenantConnectionStringStore)
            : base(options)
        {
            _multiTenancyManager = multiTenancyManager;
            _tenantConnectionStringStore = tenantConnectionStringStore;
        }

        public override string Resolve(string connectionStringName = null)
        {
            var tenant = _multiTenancyManager.CurrentTenant;
            if (tenant == null)
            {
                return base.Resolve(connectionStringName);
            }

            var connectionString = _tenantConnectionStringStore.GetConnectionStringOrNull(tenant.Id, connectionStringName);
            if (connectionString == null)
            {
                return base.Resolve(connectionStringName);
            }

            //TODO: If given tenant did not specified a connectionStringName specific connection string, then use the default connection string for connectionStringName, not tenant's default database

            return connectionString;
        }
    }
}
