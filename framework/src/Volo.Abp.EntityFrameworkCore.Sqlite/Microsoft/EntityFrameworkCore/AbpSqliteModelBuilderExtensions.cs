using Volo.Abp.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

public static class AbpSqliteModelBuilderExtensions
{
    public static void UseSqlite(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.SetDatabaseProvider(EfCoreDatabaseProvider.Sqlite);
    }
}
