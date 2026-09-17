using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore;

[DependsOn(
    typeof(AbpSettingManagementTestBaseModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
public class AbpSettingManagementEntityFrameworkCoreTestModule : AbpModule
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
            new SettingManagementDbContext(new DbContextOptionsBuilder<SettingManagementDbContext>().UseSqlite(_database.ConnectionString).Options));

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
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database?.Dispose();
    }

}
