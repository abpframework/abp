using Volo.Abp.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

public static class AbpPostgreSqlModelBuilderExtensions
{
    public static void UsePostgreSql(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.SetDatabaseProvider(EfCoreDatabaseProvider.PostgreSql);
    }
}
