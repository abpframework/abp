using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy
{
    [Dependency(ReplaceServices = true)]
    public class MultiTenantConnectionStringResolver : DefaultConnectionStringResolver
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IServiceProvider _serviceProvider;

        public MultiTenantConnectionStringResolver(
            IOptionsSnapshot<AbpDbConnectionOptions> options,
            ICurrentTenant currentTenant,
            IServiceProvider serviceProvider)
            : base(options)
        {
            _currentTenant = currentTenant;
            _serviceProvider = serviceProvider;
        }

        public override string Resolve(string connectionStringName = null)
        {
            //No current tenant, fallback to default logic
            if (_currentTenant.Id == null)
            {
                return base.Resolve(connectionStringName);
            }

            using (var serviceScope = _serviceProvider.CreateScope())
            {
                var tenantStore = serviceScope
                    .ServiceProvider
                    .GetRequiredService<ITenantStore>();

                var tenant = tenantStore.Find(_currentTenant.Id.Value);

                if (tenant?.ConnectionStrings == null)
                {
                    return base.Resolve(connectionStringName);
                }

                //Requesting default connection string
                if (connectionStringName == null)
                {
                    return tenant.ConnectionStrings.Default ??
                           Options.ConnectionStrings.Default;
                }

                //Requesting specific connection string
                var connString = tenant.ConnectionStrings.GetOrDefault(connectionStringName);
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

                return tenant.ConnectionStrings.Default ??
                       Options.ConnectionStrings.Default;
            }
        }
    }
}
