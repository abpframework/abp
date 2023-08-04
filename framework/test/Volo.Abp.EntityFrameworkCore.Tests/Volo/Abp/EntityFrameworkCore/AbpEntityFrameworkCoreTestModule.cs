using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.Domain;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.EntityFrameworkCore.TestApp.FifthContext;
using Volo.Abp.EntityFrameworkCore.TestApp.SecondContext;
using Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.TestApp.EntityFrameworkCore;
using Volo.Abp.Threading;

namespace Volo.Abp.EntityFrameworkCore;

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

        context.Services.AddAbpDbContext<HostTestAppDbContext>(options =>
        {
            options.AddDefaultRepositories(true);
            options.ReplaceDbContext<IFifthDbContext>(MultiTenancySides.Host);
        });

        context.Services.AddAbpDbContext<TenantTestAppDbContext>(options =>
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
    }

    public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
    {
        context.ServiceProvider.GetRequiredService<SecondDbContext>().Database.Migrate();

        using (var scope = context.ServiceProvider.CreateScope())
        {
            var categoryRepository = scope.ServiceProvider.GetRequiredService<IBasicRepository<Category, Guid>>();
            AsyncHelper.RunSync(async () =>
            {
                await categoryRepository.InsertManyAsync(new List<Category>
                {
                    new Category { Name = "volo.abp" },
                    new Category { Name = "abp.cli" },
                    new Category { Name = "abp.core", IsDeleted = true }
                });
            });
        }
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
