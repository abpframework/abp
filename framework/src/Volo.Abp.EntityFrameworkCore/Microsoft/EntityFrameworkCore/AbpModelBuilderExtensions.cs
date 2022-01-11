using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Microsoft.EntityFrameworkCore;

public static class AbpModelBuilderExtensions
{
    private const string ModelDatabaseProviderAnnotationKey = "_Abp_DatabaseProvider";
    private const string ModelMultiTenancySideAnnotationKey = "_Abp_MultiTenancySide";

    #region MultiTenancySide

    public static void SetMultiTenancySide(
        this ModelBuilder modelBuilder,
        MultiTenancySides side)
    {
        modelBuilder.Model.SetAnnotation(ModelMultiTenancySideAnnotationKey, side);
    }

    public static MultiTenancySides GetMultiTenancySide(this ModelBuilder modelBuilder)
    {
        var value = modelBuilder.Model[ModelMultiTenancySideAnnotationKey];
        if (value == null)
        {
            return MultiTenancySides.Both;
        }

        return (MultiTenancySides)value;
    }

    /// <summary>
    /// Returns true if this is a database schema that is used by the host
    /// but can also be shared with the tenants.
    /// </summary>
    public static bool IsHostDatabase(this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetMultiTenancySide().HasFlag(MultiTenancySides.Host);
    }

    /// <summary>
    /// Returns true if this is a database schema that is used by the tenants
    /// but can also be shared with the host.
    /// </summary>
    public static bool IsTenantDatabase(this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetMultiTenancySide().HasFlag(MultiTenancySides.Tenant);
    }

    /// <summary>
    /// Returns true if this is a database schema that is only used by the host
    /// and should not contain tenant-only tables.
    /// </summary>
    public static bool IsHostOnlyDatabase(this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetMultiTenancySide() == MultiTenancySides.Host;
    }

    /// <summary>
    /// Returns true if this is a database schema that is only used by tenants.
    /// and should not contain host-only tables.
    /// </summary>
    public static bool IsTenantOnlyDatabase(this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetMultiTenancySide() == MultiTenancySides.Tenant;
    }

    #endregion

    #region DatabaseProvider

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
        return (EfCoreDatabaseProvider?)modelBuilder.Model[ModelDatabaseProviderAnnotationKey];
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

    #endregion
}
