using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace Volo.BookStore.Data;

public class BookStoreDbMigrationService : ITransientDependency
{
    public ILogger<BookStoreDbMigrationService> Logger { get; set; }

    private readonly IDataSeeder _dataSeeder;
    private readonly IEnumerable<IBookStoreDbSchemaMigrator> _dbSchemaMigrators;
    private readonly ITenantRepository _tenantRepository;
    private readonly ICurrentTenant _currentTenant;

    public BookStoreDbMigrationService(
        IDataSeeder dataSeeder,
        IEnumerable<IBookStoreDbSchemaMigrator> dbSchemaMigrators,
        ITenantRepository tenantRepository,
        ICurrentTenant currentTenant)
    {
        _dataSeeder = dataSeeder;
        _dbSchemaMigrators = dbSchemaMigrators;
        _tenantRepository = tenantRepository;
        _currentTenant = currentTenant;

        Logger = NullLogger<BookStoreDbMigrationService>.Instance;
    }

    public async Task MigrateAsync()
    {

        Logger.LogInformation("Started database migrations...");

        await MigrateDatabaseSchemaAsync();
        await SeedDataAsync();

        Logger.LogInformation($"Successfully completed host database migrations.");

        var tenants = await _tenantRepository.GetListAsync(includeDetails: true);

        var migratedDatabaseSchemas = new HashSet<string>();
        foreach (var tenant in tenants)
        {
            using (_currentTenant.Change(tenant.Id))
            {
                if (tenant.ConnectionStrings.Any())
                {
                    var tenantConnectionStrings = tenant.ConnectionStrings
                        .Select(x => x.Value)
                        .ToList();

                    if (!migratedDatabaseSchemas.IsSupersetOf(tenantConnectionStrings))
                    {
                        await MigrateDatabaseSchemaAsync(tenant);

                        migratedDatabaseSchemas.AddIfNotContains(tenantConnectionStrings);
                    }
                }

                await SeedDataAsync(tenant);
            }

            Logger.LogInformation($"Successfully completed {tenant.Name} tenant database migrations.");
        }

        Logger.LogInformation("Successfully completed all database migrations.");
        Logger.LogInformation("You can safely end this process...");
    }

    private async Task MigrateDatabaseSchemaAsync(Tenant? tenant = null)
    {
        Logger.LogInformation(
            $"Migrating schema for {(tenant == null ? "host" : tenant.Name + " tenant")} database...");

        foreach (var migrator in _dbSchemaMigrators)
        {
            await migrator.MigrateAsync();
        }
    }

    private async Task SeedDataAsync(Tenant? tenant = null)
    {
        Logger.LogInformation($"Executing {(tenant == null ? "host" : tenant.Name + " tenant")} database seed...");

        await _dataSeeder.SeedAsync(new DataSeedContext(tenant?.Id)
            .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, IdentityDataSeedContributor.AdminEmailDefaultValue)
            .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, IdentityDataSeedContributor.AdminPasswordDefaultValue)
        );
    }

}
