using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;

namespace MyCompanyName.MyProjectName.Data
{
    public class MyProjectNameDbMigrationService : ITransientDependency
    {
        public ILogger<MyProjectNameDbMigrationService> Logger { get; set; }

        private readonly IDataSeeder _dataSeeder;
        private readonly IMyProjectNameDbSchemaMigrator _dbSchemaMigrator;
        private readonly ITenantRepository _tenantRepository;
        private readonly ICurrentTenant _currentTenant;

        public MyProjectNameDbMigrationService(
            IDataSeeder dataSeeder,
            IMyProjectNameDbSchemaMigrator dbSchemaMigrator,
            ITenantRepository tenantRepository,
            ICurrentTenant currentTenant)
        {
            _dataSeeder = dataSeeder;
            _dbSchemaMigrator = dbSchemaMigrator;
            _tenantRepository = tenantRepository;
            _currentTenant = currentTenant;

            Logger = NullLogger<MyProjectNameDbMigrationService>.Instance;
        }

        public async Task MigrateAsync()
        {
            Logger.LogInformation("Started database migrations...");

            await MigrateHostDatabaseAsync();

            foreach (var tenant in await _tenantRepository.GetListAsync())
            {
                using (_currentTenant.Change(tenant.Id))
                {

                    await MigrateTenantDatabasesAsync(tenant);
                }
            }
            Logger.LogInformation("Successfully completed database migrations.");
        }
        private async Task MigrateHostDatabaseAsync()
        {
            Logger.LogInformation("Migrating host database schema...");
            await _dbSchemaMigrator.MigrateAsync();

            Logger.LogInformation("Executing host database seed...");
            await _dataSeeder.SeedAsync();

            Logger.LogInformation("Successfully completed host database migrations.");
        }
        private async Task MigrateTenantDatabasesAsync(Tenant tenant)
        {
            Logger.LogInformation($"Migrating {tenant.Name} database schema...");
            await _dbSchemaMigrator.MigrateAsync();

            Logger.LogInformation($"Executing {tenant.Name} database seed...");
            await _dataSeeder.SeedAsync(tenant.Id);
            Logger.LogInformation($"Successfully completed {tenant.Name} database migrations.");
        }
    }
}