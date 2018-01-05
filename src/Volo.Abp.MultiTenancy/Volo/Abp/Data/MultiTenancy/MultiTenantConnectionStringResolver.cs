using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Data.MultiTenancy
{
    [Dependency(ReplaceServices = true)]
    public class MultiTenantConnectionStringResolver : DefaultConnectionStringResolver
    {
        private readonly IMultiTenancyManager _multiTenancyManager;

        public MultiTenantConnectionStringResolver(
            IOptionsSnapshot<DbConnectionOptions> options,
            IMultiTenancyManager multiTenancyManager)
            : base(options)
        {
            _multiTenancyManager = multiTenancyManager;
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
                return tenant.FindDefaultConnectionString() ??
                       Options.ConnectionStrings.Default;
            }

            //Requesting specific connection string
            var connString = tenant.FindConnectionString(connectionStringName);
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

            return tenant.FindDefaultConnectionString() ??
                   Options.ConnectionStrings.Default;
        }
    }
}
