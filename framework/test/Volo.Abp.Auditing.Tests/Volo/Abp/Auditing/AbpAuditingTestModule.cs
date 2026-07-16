using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Auditing.App.Entities;
using Volo.Abp.Auditing.App.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace Volo.Abp.Auditing;

[DependsOn(
    typeof(AbpTestBaseModule),
    typeof(AbpAutofacModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
)]
public class AbpAuditingTestModule : AbpModule
{
    private AbpUnitTestSqliteDatabase _database;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<AbpAuditingTestDbContext>(options =>
        {
            options.AddDefaultRepositories(true);
            options.Entity<AppEntityWithNavigations>(opt =>
            {
                opt.DefaultWithDetailsFunc = q => q.Include(p => p.OneToOne).Include(p => p.OneToMany).Include(p => p.ManyToMany);
            });
        });

        _database = new AbpUnitTestSqliteDatabase();
        _database.CreateTables(
            new AbpAuditingTestDbContext(new DbContextOptionsBuilder<AbpAuditingTestDbContext>().UseSqlite(_database.ConnectionString).AddAbpDbContextOptionsExtension().Options));

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

        Configure<AbpAuditingOptions>(options =>
        {
            options.EntityHistorySelectors.Add(
                new NamedTypeSelector(
                    "AppEntityWithSelector",
                    type => type == typeof(AppEntityWithSelector))
            );

            options.EntityHistorySelectors.Add(
                new NamedTypeSelector(
                    "AppEntityWithSoftDelete",
                    type => type == typeof(AppEntityWithSoftDelete))
            );

            options.EntityHistorySelectors.Add(
                new NamedTypeSelector(
                    "AppEntityWithValueObject",
                    type => type == typeof(AppEntityWithValueObject) || type == typeof(AppEntityWithValueObjectAddress))
            );

            options.EntityHistorySelectors.Add(new NamedTypeSelector(nameof(AppEntityWithJsonProperty), type => type == typeof(AppEntityWithJsonProperty)));
            options.EntityHistorySelectors.Add(new NamedTypeSelector(nameof(AppEntityWithComplexProperty), type => type == typeof(AppEntityWithComplexProperty)));
        });

        context.Services.AddType<Auditing_Tests.MyAuditedObject1>();
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database?.Dispose();
    }

}
