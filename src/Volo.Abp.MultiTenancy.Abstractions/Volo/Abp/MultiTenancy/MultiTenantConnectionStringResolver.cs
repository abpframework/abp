using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    [Dependency(ReplaceServices = true)]
    public class MultiTenantConnectionStringResolver : DefaultConnectionStringResolver
    {
        private readonly ICurrentTenant _currentTenant;

        public MultiTenantConnectionStringResolver(
            IOptionsSnapshot<DbConnectionOptions> options,
            ICurrentTenant currentTenant)
            : base(options)
        {
            _currentTenant = currentTenant;
        }

        public override string Resolve(string connectionStringName = null)
        {
            var tenantConnectionStrings = _currentTenant.ConnectionStrings;

            //No current tenant, fallback to default logic
            if (tenantConnectionStrings == null)
            {
                return base.Resolve(connectionStringName);
            }

            //Requesting default connection string
            if (connectionStringName == null)
            {
                return tenantConnectionStrings.Default ??
                       Options.ConnectionStrings.Default;
            }

            //Requesting specific connection string
            var connString = tenantConnectionStrings.GetOrDefault(connectionStringName);
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

            return tenantConnectionStrings.Default ??
                   Options.ConnectionStrings.Default;
        }
    }
}
