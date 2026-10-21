using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

[DependsOn(typeof(AbpEntityFrameworkCoreSqliteModule), typeof(AbpAutofacModule))]
public class TenantDependentTranslationTestModule : CompiledQueryCacheTestModuleBase
{
    public static readonly (string Name, Guid? OwnerTenantId)[] CustomRows =
    {
        ("A-row", TenantA),
        ("B-row", TenantB),
        ("Host-row", null)
    };

    protected override bool UseDbFunction => true;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
        context.Services.AddAbpDbContext<TenantDependentTranslationTestDbContext>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        base.OnApplicationInitialization(context);

        using var scope = context.ServiceProvider.CreateScope();
        using var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin();
        var dbContext = scope.ServiceProvider.GetRequiredService<TenantDependentTranslationTestDbContext>();
        dbContext.GetService<IRelationalDatabaseCreator>().CreateTables();
        foreach (var row in CustomRows)
        {
            dbContext.Database.ExecuteSqlInterpolated(
                $@"INSERT INTO CustomTranslationRows (Id, OwnerTenantId, Name)
                   VALUES ({Guid.NewGuid()}, {row.OwnerTenantId}, {row.Name})");
        }
    }
}
