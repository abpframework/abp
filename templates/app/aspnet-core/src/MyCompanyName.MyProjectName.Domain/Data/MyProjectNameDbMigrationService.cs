using System.Collections.Generic;
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
        private readonly IEnumerable<IMyProjectNameDbSchemaMigrator> _dbSchemaMigrators;
        private readonly ITenantRepository _tenantRepository;
        private readonly ICurrentTenant _currentTenant;

        public MyProjectNameDbMigrationService(
            IDataSeeder dataSeeder,
            IEnumerable<IMyProjectNameDbSchemaMigrator> dbSchemaMigrators,
            ITenantRepository tenantRepository,
            ICurrentTenant currentTenant)
        {
            _dataSeeder = dataSeeder;
            _dbSchemaMigrators = dbSchemaMigrators;
            _tenantRepository = tenantRepository;
            _currentTenant = currentTenant;

            Logger = NullLogger<MyProjectNameDbMigrationService>.Instance;
        }

        public async Task MigrateAsync()
        {
            Logger.LogInformation("Started database migrations...");

            await MigrateDatabaseAsync();

            var tenants = await _tenantRepository.GetListAsync(includeDetails: true);

            var migratedDatabases = new HashSet<string>();
            foreach (var tenant in tenants.Where(t => t.ConnectionStrings.Any()))
            {
                var tenantConnectionStrings = tenant.ConnectionStrings.Select(x => x.Value).ToList();
                
                if (!migratedDatabases.Any() || !migratedDatabases.IsSupersetOf(tenantConnectionStrings))
                {
                    using (_currentTenant.Change(tenant.Id))
                    {
                        await MigrateDatabaseAsync(tenant);
                    }

                    tenantConnectionStrings.ForEach(x => migratedDatabases.Add(x));
                }
            }

            Logger.LogInformation("Successfully completed database migrations.");
        }

        private async Task MigrateDatabaseAsync(Tenant tenant = null)
        {
            var migrateName = tenant == null ? "host" : tenant.Name + " tenant";

            Logger.LogInformation($"Migrating schema for {migrateName} database...");

            foreach (var migrator in _dbSchemaMigrators)
            {
                await migrator.MigrateAsync();
            }

            Logger.LogInformation($"Executing {migrateName} database seed...");

            await _dataSeeder.SeedAsync(tenant?.Id);

            Logger.LogInformation($"Successfully completed {migrateName} database migrations.");
        }
    }
}