using System;
using System.Threading.Tasks;
using Volo.Abp.Data;

namespace Volo.Abp.TenantManagement
{
    public static class DataSeederExtensions
    {
        public static Task SeedAsync(this IDataSeeder seeder, Guid? tenantId, TenantCreateDto input)
        {
            var context = new DataSeedContext(tenantId)
                .WithProperty("AdminEmail", input.AdminEmailAddress)
                .WithProperty("AdminPassword", input.AdminPassword);

            return seeder.SeedAsync(context);
        }
    }
}
