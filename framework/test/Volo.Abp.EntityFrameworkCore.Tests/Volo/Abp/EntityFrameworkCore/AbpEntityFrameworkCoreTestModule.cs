using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore.Domain;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.EntityFrameworkCore.TestApp.SecondContext;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Volo.Abp.Timing;

namespace Volo.Abp.EntityFrameworkCore
{
    [DependsOn(typeof(AbpEntityFrameworkCoreSqliteModule))]
    [DependsOn(typeof(TestAppModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(AbpEfCoreTestSecondContextModule))]
    public class AbpEntityFrameworkCoreTestModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            TestEntityExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<TestAppDbContext>(options =>
            {
                options.AddDefaultRepositories(true);
                options.ReplaceDbContext<IThirdDbContext>();

                options.Entity<Person>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Phones);
                });

                options.Entity<Author>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Books);
                });
            });

            var sqliteConnection = CreateDatabaseAndGetConnection();

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.DbContextOptions.UseSqlite(sqliteConnection);
                });
            });

            Configure<AbpClockOptions>(options => options.Kind = DateTimeKind.Utc);
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider.GetRequiredService<SecondDbContext>().Database.Migrate();
        }

        private static SqliteConnection CreateDatabaseAndGetConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            using (var context = new TestMigrationsDbContext(new DbContextOptionsBuilder<TestMigrationsDbContext>().UseSqlite(connection).Options))
            {
                context.GetService<IRelationalDatabaseCreator>().CreateTables();
                context.Database.ExecuteSqlRaw(
                    @"CREATE VIEW View_PersonView AS 
                      SELECT Name, CreationTime, Birthday, LastActive FROM People");
            }

            return connection;
        }
    }
}
