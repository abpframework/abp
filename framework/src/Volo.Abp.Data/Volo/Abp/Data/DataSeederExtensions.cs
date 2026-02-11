using System;
using System.Threading.Tasks;
using Volo.Abp.Uow;

namespace Volo.Abp.Data;

public static class DataSeederExtensions
{
    public const string SeedInSeparateUow = "__SeedInSeparateUow";
    public const string SeedInSeparateUowOptions = "__SeedInSeparateUowOptions";
    public const string SeedInSeparateUowRequiresNew = "__SeedInSeparateUowRequiresNew";

    public static Task SeedAsync(this IDataSeeder seeder, Guid? tenantId = null)
    {
        return seeder.SeedAsync(new DataSeedContext(tenantId));
    }

    public static Task SeedInSeparateUowAsync(this IDataSeeder seeder, Guid? tenantId = null, AbpUnitOfWorkOptions? options = null, bool requiresNew = false)
    {
        var context = new DataSeedContext(tenantId);
        context.WithProperty(SeedInSeparateUow, true);
        context.WithProperty(SeedInSeparateUowOptions, options);
        context.WithProperty(SeedInSeparateUowRequiresNew, requiresNew);
        return seeder.SeedAsync(context);
    }
}
