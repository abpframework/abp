using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpAuditLoggingTestBaseModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
    public class AbpAuditLoggingEntityFrameworkCoreTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var sqliteConnection = CreateDatabaseAndGetConnection();

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(ctx =>
                {
                    ctx.DbContextOptions.UseSqlite(sqliteConnection);
                });
            });
        }

        private static SqliteConnection CreateDatabaseAndGetConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            new AbpAuditLoggingDbContext(
                new DbContextOptionsBuilder<AbpAuditLoggingDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();

            return connection;
        }
    }
}