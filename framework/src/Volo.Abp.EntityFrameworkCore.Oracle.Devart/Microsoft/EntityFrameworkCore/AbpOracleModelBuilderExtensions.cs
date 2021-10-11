using Volo.Abp.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore
{
    public static class AbpOracleModelBuilderExtensions
    {
        public static void UseOracle(
            this ModelBuilder modelBuilder)
        {
            modelBuilder.SetDatabaseProvider(EfCoreDatabaseProvider.Oracle);
        }
    }
}
