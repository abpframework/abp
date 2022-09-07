using System;
using System.Threading.Tasks;

namespace Volo.Abp.Data;

public static class DataSeederExtensions
{
    public static Task SeedAsync(this IDataSeeder seeder, Guid? tenantId = null)
    {
        return seeder.SeedAsync(new DataSeedContext(tenantId));
    }

    public static Task SeedInSeparateUowAsync(this IDataSeeder seeder, Guid? tenantId = null)
    {
        return seeder.SeedAsync(new DataSeedContext(tenantId).WithProperty(nameof(DataSeederExtensions.SeedInSeparateUowAsync), true));
    }
}
