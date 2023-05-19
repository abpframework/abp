using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiTenancy;

[Dependency(ReplaceServices = true)]
public class MultiTenantConnectionStringResolver : DefaultConnectionStringResolver
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IServiceProvider _serviceProvider;

    public MultiTenantConnectionStringResolver(
        IOptionsMonitor<AbpDbConnectionOptions> options,
        ICurrentTenant currentTenant,
        IServiceProvider serviceProvider)
        : base(options)
    {
        _currentTenant = currentTenant;
        _serviceProvider = serviceProvider;
    }

    public override async Task<string> ResolveAsync(string connectionStringName = null)
    {
        if (_currentTenant.Id == null)
        {
            //No current tenant, fallback to default logic
            return await base.ResolveAsync(connectionStringName);
        }

        var tenant = await FindTenantConfigurationAsync(_currentTenant.Id.Value);

        if (tenant == null || tenant.ConnectionStrings.IsNullOrEmpty())
        {
            //Tenant has not defined any connection string, fallback to default logic
            return await base.ResolveAsync(connectionStringName);
        }

        var tenantDefaultConnectionString = tenant.ConnectionStrings.Default;

        //Requesting default connection string...
        if (connectionStringName == null ||
            connectionStringName == ConnectionStrings.DefaultConnectionStringName)
        {
            //Return tenant's default or global default
            return !tenantDefaultConnectionString.IsNullOrWhiteSpace()
                ? tenantDefaultConnectionString
                : Options.ConnectionStrings.Default;
        }

        //Requesting specific connection string...
        var connString = tenant.ConnectionStrings.GetOrDefault(connectionStringName);
        if (!connString.IsNullOrWhiteSpace())
        {
            //Found for the tenant
            return connString;
        }

        //Fallback to the mapped database for the specific connection string
        var database = Options.Databases.GetMappedDatabaseOrNull(connectionStringName);
        if (database != null && database.IsUsedByTenants)
        {
            connString = tenant.ConnectionStrings.GetOrDefault(database.DatabaseName);
            if (!connString.IsNullOrWhiteSpace())
            {
                //Found for the tenant
                return connString;
            }
        }

        //Fallback to tenant's default connection string if available
        if (!tenantDefaultConnectionString.IsNullOrWhiteSpace())
        {
            return tenantDefaultConnectionString;
        }

        return await base.ResolveAsync(connectionStringName);
    }

    [Obsolete("Use ResolveAsync method.")]
    public override string Resolve(string connectionStringName = null)
    {
        if (_currentTenant.Id == null)
        {
            //No current tenant, fallback to default logic
            return base.Resolve(connectionStringName);
        }

        var tenant = FindTenantConfiguration(_currentTenant.Id.Value);

        if (tenant == null || tenant.ConnectionStrings.IsNullOrEmpty())
        {
            //Tenant has not defined any connection string, fallback to default logic
            return base.Resolve(connectionStringName);
        }

        var tenantDefaultConnectionString = tenant.ConnectionStrings.Default;

        //Requesting default connection string...
        if (connectionStringName == null ||
            connectionStringName == ConnectionStrings.DefaultConnectionStringName)
        {
            //Return tenant's default or global default
            return !tenantDefaultConnectionString.IsNullOrWhiteSpace()
                ? tenantDefaultConnectionString
                : Options.ConnectionStrings.Default;
        }

        //Requesting specific connection string...
        var connString = tenant.ConnectionStrings.GetOrDefault(connectionStringName);
        if (!connString.IsNullOrWhiteSpace())
        {
            //Found for the tenant
            return connString;
        }

        //Fallback to tenant's default connection string if available
        if (!tenantDefaultConnectionString.IsNullOrWhiteSpace())
        {
            return tenantDefaultConnectionString;
        }

        //Try to find the specific connection string for given name
        var connStringInOptions = Options.ConnectionStrings.GetOrDefault(connectionStringName);
        if (!connStringInOptions.IsNullOrWhiteSpace())
        {
            return connStringInOptions;
        }

        //Fallback to the global default connection string
        var defaultConnectionString = Options.ConnectionStrings.Default;
        if (!defaultConnectionString.IsNullOrWhiteSpace())
        {
            return defaultConnectionString;
        }

        throw new AbpException("No connection string defined!");
    }

    protected virtual async Task<TenantConfiguration> FindTenantConfigurationAsync(Guid tenantId)
    {
        using (var serviceScope = _serviceProvider.CreateScope())
        {
            var tenantStore = serviceScope
                .ServiceProvider
                .GetRequiredService<ITenantStore>();

            return await tenantStore.FindAsync(tenantId);
        }
    }

    [Obsolete("Use FindTenantConfigurationAsync method.")]
    protected virtual TenantConfiguration FindTenantConfiguration(Guid tenantId)
    {
        using (var serviceScope = _serviceProvider.CreateScope())
        {
            var tenantStore = serviceScope
                .ServiceProvider
                .GetRequiredService<ITenantStore>();

            return tenantStore.Find(tenantId);
        }
    }
}
