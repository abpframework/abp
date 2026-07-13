using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore;

[DependsOn(
    typeof(OpenIddictTestBaseModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule)
    )]
public class OpenIddictEntityFrameworkCoreTestModule : AbpModule
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
            new IdentityDbContext(new DbContextOptionsBuilder<IdentityDbContext>().UseSqlite(_database.ConnectionString).Options),
            new OpenIddictDbContext(new DbContextOptionsBuilder<OpenIddictDbContext>().UseSqlite(_database.ConnectionString).Options),
            new PermissionManagementDbContext(new DbContextOptionsBuilder<PermissionManagementDbContext>().UseSqlite(_database.ConnectionString).Options));

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
