using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing.App.Entities;
using Volo.Abp.Auditing.App.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Auditing
{
    [DependsOn(
        typeof(AbpTestBaseModule),
        typeof(AbpAutofacModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class AbpAuditingTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AbpAuditingTestDbContext>(options =>
            {
                options.AddDefaultRepositories(true);
            });

            var sqliteConnection = CreateDatabaseAndGetConnection();

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.DbContextOptions.UseSqlite(sqliteConnection);
                });
            });

            Configure<AbpAuditingOptions>(options =>
            {
                options.EntityHistorySelectors.Add(
                    new NamedTypeSelector(
                        "AppEntityWithSelector",
                        type => type == typeof(AppEntityWithSelector))
                );
            });

            context.Services.AddType<Auditing_Tests.MyAuditedObject1>();
        }

        private static SqliteConnection CreateDatabaseAndGetConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            using (var context = new AbpAuditingTestDbContext(new DbContextOptionsBuilder<AbpAuditingTestDbContext>()
                .UseSqlite(connection).Options))
            {
                context.GetService<IRelationalDatabaseCreator>().CreateTables();
            }

            return connection;
        }
    }
}