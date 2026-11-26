using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

public abstract class DerivedEntityFilterTestModuleBase<TDbContext> : AbpModule
    where TDbContext : AbpDbContext<TDbContext>
{
    private readonly AbpUnitTestSqliteDatabase _database = new();

    protected abstract bool UseDbFunction { get; }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TDbContext>(options =>
        {
            options.AddDefaultRepositories(true);
        });

        Configure<AbpEfCoreGlobalFilterOptions>(options => options.UseDbFunction = UseDbFunction);
        Configure<AbpDbConnectionOptions>(options => options.ConnectionStrings.Default = _database.ConnectionString);
        Configure<AbpDbContextOptions>(options => options.Configure(configuration => configuration.UseSqlite()));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        using var scope = context.ServiceProvider.CreateScope();
        using var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin();
        scope.ServiceProvider.GetRequiredService<TDbContext>().Database.EnsureCreated();
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _database.Dispose();
    }
}
