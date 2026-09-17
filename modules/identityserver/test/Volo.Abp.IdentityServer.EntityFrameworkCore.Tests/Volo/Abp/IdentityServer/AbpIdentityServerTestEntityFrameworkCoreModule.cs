using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.IdentityServer;

[DependsOn(
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpIdentityServerEntityFrameworkCoreModule),
    typeof(AbpIdentityServerTestBaseModule),
    typeof(AbpEntityFrameworkCoreSqliteModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule)
    )]
public class AbpIdentityServerTestEntityFrameworkCoreModule : AbpModule
{
    private AbpUnitTestSqliteDatabase _database;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        _database = new AbpUnitTestSqliteDatabase();
        _database.CreateTables(
            new IdentityDbContext(new DbContextOptionsBuilder<IdentityDbContext>().UseSqlite(_database.ConnectionString).Options),
            new IdentityServerDbContext(new DbContextOptionsBuilder<IdentityServerDbContext>().UseSqlite(_database.ConnectionString).Options),
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
