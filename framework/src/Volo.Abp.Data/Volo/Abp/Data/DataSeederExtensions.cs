using System;
using System.Threading.Tasks;

namespace Volo.Abp.Data
{
    public static class DataSeederExtensions
    {
        public static Task SeedAsync(this IDataSeeder seeder, Guid? tenantId, string adminEmailAddress, string adminPassword)
        {
            var context = new DataSeedContext(tenantId)
                                .WithProperty("AdminEmail", adminEmailAddress)
                                .WithProperty("AdminPassword", adminPassword);

            return seeder.SeedAsync(context);
        }
    }
}