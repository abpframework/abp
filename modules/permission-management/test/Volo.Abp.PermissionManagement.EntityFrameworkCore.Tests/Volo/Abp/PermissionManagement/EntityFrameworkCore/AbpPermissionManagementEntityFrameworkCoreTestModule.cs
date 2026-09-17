using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.PermissionManagement.EntityFrameworkCore;

[DependsOn(
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementTestBaseModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
)]
public class AbpPermissionManagementEntityFrameworkCoreTestModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpSqliteOptions>(x => x.BusyTimeout = null);
    }

    private AbpUnitTestSqliteDatabase _database;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        _database = new AbpUnitTestSqliteDatabase();
        _database.CreateTables(
            new PermissionManagementDbContext(
                new DbContextOptionsBuilder<PermissionManagementDbContext>().UseSqlite(_database.ConnectionString).Options));

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

        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled; //EF in-memory database does not support transactions
        });

        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database?.Dispose();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();
        var initializer = rootServiceProvider.GetRequiredService<PermissionDynamicInitializer>();
        AsyncHelper.RunSync(() => initializer.InitializeAsync(false));
    }
}
