using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity.EntityFrameworkCore;

// EF/SQLite equivalent of the MongoDB separate-database test module: each predefined tenant
// has its own keep-alive in-memory SQLite connection. Each test method (and therefore each
// AbpApplication) gets a unique connection-string suffix so the test-data seeder runs into
// fresh databases instead of duplicating into shared cache.
[DependsOn(
    typeof(AbpIdentityTestBaseModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule))]
public class AbpIdentitySharedUserSeparateDbEntityFrameworkCoreTestModule : AbpModule
{
    public static readonly Guid TenantAId = IdentitySharedUserSeparateDbConstants.TenantAId;
    public static readonly Guid TenantBId = IdentitySharedUserSeparateDbConstants.TenantBId;

    // Per-app keep-alive connections so the in-memory SQLite databases survive for the test's
    // lifetime (without an open connection, shared-cache in-memory databases are discarded).
    // Uses AbpUnitTestSqliteConnection (SemaphoreSlim around CreateCommand) — SQLite isn't
    // thread-safe and parallel xUnit collections would otherwise race. Disposed in
    // OnApplicationShutdown so connections do not accumulate across tests.
    private readonly List<AbpUnitTestSqliteConnection> _keepAlive = new();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpSqliteOptions>(x => x.BusyTimeout = null);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Unique-per-app suffix so each test method gets a fresh trio of databases (the seeder
        // in AbpIdentityTestBaseModule.OnApplicationInitialization expects to write into empty
        // tables, which would fail if test methods reused the same shared-cache database).
        var suffix = Guid.NewGuid().ToString("N");
        var hostConnection = $"Data Source=AbpIdentity_SeparateDb_Host_{suffix};Mode=Memory;Cache=Shared";
        var tenantAConnection = $"Data Source=AbpIdentity_SeparateDb_TenantA_{suffix};Mode=Memory;Cache=Shared";
        var tenantBConnection = $"Data Source=AbpIdentity_SeparateDb_TenantB_{suffix};Mode=Memory;Cache=Shared";

        EnsureDatabase(hostConnection);
        EnsureDatabase(tenantAConnection);
        EnsureDatabase(tenantBConnection);

        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = hostConnection;
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(ctx =>
            {
                ctx.DbContextOptions.UseSqlite(ctx.ConnectionString);
            });
        });

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
            options.UserSharingStrategy = TenantUserSharingStrategy.Shared;
        });

        Configure<AbpDefaultTenantStoreOptions>(options =>
        {
            options.Tenants = new[]
            {
                new TenantConfiguration(TenantAId, "tenant-a")
                {
                    ConnectionStrings = new ConnectionStrings
                    {
                        { ConnectionStrings.DefaultConnectionStringName, tenantAConnection }
                    }
                },
                new TenantConfiguration(TenantBId, "tenant-b")
                {
                    ConnectionStrings = new ConnectionStrings
                    {
                        { ConnectionStrings.DefaultConnectionStringName, tenantBConnection }
                    }
                }
            };
        });

        context.Services.AddAlwaysDisableUnitOfWorkTransaction();
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        foreach (var connection in _keepAlive)
        {
            connection.Dispose();
        }
        _keepAlive.Clear();
    }

    private void EnsureDatabase(string connectionString)
    {
        var keepAlive = new AbpUnitTestSqliteConnection(connectionString);
        keepAlive.Open();
        _keepAlive.Add(keepAlive);

        new IdentityDbContext(
            new DbContextOptionsBuilder<IdentityDbContext>().UseSqlite(connectionString).Options)
            .GetService<IRelationalDatabaseCreator>().CreateTables();

        new PermissionManagementDbContext(
            new DbContextOptionsBuilder<PermissionManagementDbContext>().UseSqlite(connectionString).Options)
            .GetService<IRelationalDatabaseCreator>().CreateTables();
    }
}
