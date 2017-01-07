using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.DependencyInjection;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.Data.MultiTenancy
{
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

            //No current tenant, fallback to default logic
            if (tenant == null)
            {
                return base.Resolve(connectionStringName);
            }

            //Requesting default connection string
            if (connectionStringName == null)
            {
                return _tenantConnectionStringStore.GetDefaultConnectionStringOrNull(tenant.Id) ??
                       Options.ConnectionStrings.Default;
            }

            //Requesting specific connection string
            var connString = _tenantConnectionStringStore.GetConnectionStringOrNull(tenant.Id, connectionStringName);
            if (connString != null)
            {
                return connString;
            }

            /* Requested a specific connection string, but it's not specified for the tenant.
             * - If it's specified in options, use it.
             * - If not, use tenant's default conn string.
             */
                   
            var connStringInOptions = Options.ConnectionStrings.GetOrDefault(connectionStringName);
            if (connStringInOptions != null)
            {
                return connStringInOptions;
            }

            return _tenantConnectionStringStore.GetDefaultConnectionStringOrNull(tenant.Id) ??
                   Options.ConnectionStrings.Default;
        }
    }
}
