using Volo.Abp.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore
{
    public static class AbpModelBuilderExtensions
    {
        private const string ModelDatabaseProviderAnnotationKey = "_Abp_DatabaseProvider";

        public static void SetDatabaseProvider(
            this ModelBuilder modelBuilder,
            EfCoreDatabaseProvider databaseProvider)
        {
            modelBuilder.Model.SetAnnotation(ModelDatabaseProviderAnnotationKey, databaseProvider);
        }

        public static void ClearDatabaseProvider(
            this ModelBuilder modelBuilder)
        {
            modelBuilder.Model.RemoveAnnotation(ModelDatabaseProviderAnnotationKey);
        }

        public static EfCoreDatabaseProvider? GetDatabaseProvider(
            this ModelBuilder modelBuilder
        )
        {
            return (EfCoreDatabaseProvider?) modelBuilder.Model[ModelDatabaseProviderAnnotationKey];
        }

        public static bool IsUsingMySQL(
            this ModelBuilder modelBuilder)
        {
            return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.MySql;
        }

        public static bool IsUsingOracle(
            this ModelBuilder modelBuilder)
        {
            return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.Oracle;
        }

        public static bool IsUsingSqlServer(
            this ModelBuilder modelBuilder)
        {
            return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.SqlServer;
        }

        public static bool IsUsingPostgreSql(
            this ModelBuilder modelBuilder)
        {
            return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.PostgreSql;
        }

        public static bool IsUsingSqlite(
            this ModelBuilder modelBuilder)
        {
            return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.Sqlite;
        }

        public static void UseInMemory(
            this ModelBuilder modelBuilder)
        {
            modelBuilder.SetDatabaseProvider(EfCoreDatabaseProvider.InMemory);
        }
        
        public static bool IsUsingInMemory(
            this ModelBuilder modelBuilder)
        {
            return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.InMemory;
        }

        public static void UseCosmos(
            this ModelBuilder modelBuilder)
        {
            modelBuilder.SetDatabaseProvider(EfCoreDatabaseProvider.Cosmos);
        }
        
        public static bool IsUsingCosmos(
            this ModelBuilder modelBuilder)
        {
            return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.Cosmos;
        }

        public static void UseFirebird(
            this ModelBuilder modelBuilder)
        {
            modelBuilder.SetDatabaseProvider(EfCoreDatabaseProvider.Firebird);
        }
        
        public static bool IsUsingFirebird(
            this ModelBuilder modelBuilder)
        {
            return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.Firebird;
        }
    }
}