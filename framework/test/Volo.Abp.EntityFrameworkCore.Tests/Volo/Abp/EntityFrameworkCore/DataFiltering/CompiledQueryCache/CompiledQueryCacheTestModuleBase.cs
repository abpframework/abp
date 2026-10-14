using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public abstract class CompiledQueryCacheTestModuleBase : AbpModule
{
    public static readonly Guid TenantA = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static readonly Guid TenantB = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    public static readonly (string Name, Guid? TenantId, bool Deleted)[] Rows =
    {
        ("A-live", TenantA, false),
        ("A-deleted", TenantA, true),
        ("B-live", TenantB, false),
        ("B-deleted", TenantB, true),
        ("Host-live", null, false),
        ("Host-deleted", null, true)
    };

    private readonly AbpUnitTestSqliteDatabase _database = new();
    private ServiceProvider _efServiceProvider;

    protected abstract bool UseDbFunction { get; }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var counter = new QueryCompilationCounter();

        // Avoid growing EF's global service-provider cache across test applications.
        var efServices = new ServiceCollection();
        efServices.AddEntityFrameworkSqlite();
        if (UseDbFunction)
        {
            new AbpDbContextOptionsExtension().ApplyServices(efServices);
        }

        _efServiceProvider = efServices.BuildServiceProvider();
        context.Services.AddSingleton(counter);
        context.Services.AddAbpDbContext<CompiledQueryCacheTestDbContext>();
        Configure<AbpEfCoreGlobalFilterOptions>(options => options.UseDbFunction = UseDbFunction);
        Configure<AbpDbConnectionOptions>(options => options.ConnectionStrings.Default = _database.ConnectionString);
        Configure<AbpDbContextOptions>(options => options.Configure(configuration =>
        {
            configuration.UseSqlite();
            configuration.DbContextOptions.UseLoggerFactory(null);
            configuration.DbContextOptions.UseInternalServiceProvider(_efServiceProvider);
            configuration.DbContextOptions.LogTo(
                _ => counter.Increment(),
                new[] { CoreEventId.QueryCompilationStarting });
        }));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        using var scope = context.ServiceProvider.CreateScope();
        using var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin();
        var dbContext = scope.ServiceProvider.GetRequiredService<CompiledQueryCacheTestDbContext>();
        dbContext.Database.EnsureCreated();
        foreach (var row in Rows)
        {
            dbContext.Database.ExecuteSqlInterpolated(
                $@"INSERT INTO Rows (Id, TenantId, IsDeleted, Name)
                   VALUES ({Guid.NewGuid()}, {row.TenantId}, {row.Deleted}, {row.Name})");
        }
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database.Dispose();
        _efServiceProvider.Dispose();
    }
}
