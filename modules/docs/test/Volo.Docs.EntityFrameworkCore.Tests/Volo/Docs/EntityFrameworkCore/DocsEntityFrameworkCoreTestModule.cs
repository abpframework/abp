using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace Volo.Docs.EntityFrameworkCore
{
    [DependsOn(
        typeof(DocsTestBaseModule),
        typeof(DocsEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqliteModule)
        )]
    public class DocsEntityFrameworkCoreTestModule : AbpModule
    {
        private AbpUnitTestSqliteDatabase _database;

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpSqliteOptions>(x => x.BusyTimeout = null);
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            _database = new AbpUnitTestSqliteDatabase();
            _database.CreateTables(
                new DocsDbContext(new DbContextOptionsBuilder<DocsDbContext>().UseSqlite(_database.ConnectionString).Options));

            Configure<AbpDbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = _database.ConnectionString;
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.UseSqlite();
                });
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _database?.Dispose();
        }

    }
}
