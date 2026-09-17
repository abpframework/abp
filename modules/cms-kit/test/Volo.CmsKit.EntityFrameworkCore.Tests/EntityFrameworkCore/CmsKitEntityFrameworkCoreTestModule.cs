using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Volo.CmsKit.EntityFrameworkCore;

[DependsOn(
    typeof(CmsKitTestBaseModule),
    typeof(CmsKitEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule)
    )]
public class CmsKitEntityFrameworkCoreTestModule : AbpModule
{
    private AbpUnitTestSqliteDatabase _database;

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpSqliteOptions>(x => x.BusyTimeout = null);
        context.Services.AddDataMigrationEnvironment();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        _database = new AbpUnitTestSqliteDatabase();
        _database.CreateTables(
            new CmsKitDbContext(new DbContextOptionsBuilder<CmsKitDbContext>().UseSqlite(_database.ConnectionString).Options),
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
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database?.Dispose();
    }

}
