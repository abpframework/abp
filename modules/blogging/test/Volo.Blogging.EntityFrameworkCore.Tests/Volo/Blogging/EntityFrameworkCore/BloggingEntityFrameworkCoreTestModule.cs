using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace Volo.Blogging.EntityFrameworkCore
{
    [DependsOn(
        typeof(BloggingEntityFrameworkCoreModule),
        typeof(BloggingTestBaseModule),
        typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
    public class BloggingEntityFrameworkCoreTestModule : AbpModule
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
                new BloggingDbContext(new DbContextOptionsBuilder<BloggingDbContext>().UseSqlite(_database.ConnectionString).Options));

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