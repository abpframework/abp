using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore;

public class EfCoreDatabaseProviderHelper_Tests
{
    [Fact]
    public void Should_Detect_SqlServer_From_Real_Assembly()
    {
        var builder = new DbContextOptionsBuilder<EmptyDbContext>();
        Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsExtensions.UseSqlServer(builder, "Server=localhost;Database=test");
        using var context = new EmptyDbContext(builder.Options);
        EfCoreDatabaseProviderHelper.GetDatabaseProviderOrNull(context.Database.ProviderName)
            .ShouldBe(EfCoreDatabaseProvider.SqlServer);
    }

    [Fact]
    public void Should_Detect_PostgreSql_From_Real_Assembly()
    {
        var builder = new DbContextOptionsBuilder<EmptyDbContext>();
        Microsoft.EntityFrameworkCore.NpgsqlDbContextOptionsBuilderExtensions.UseNpgsql(builder, "Host=localhost;Database=test");
        using var context = new EmptyDbContext(builder.Options);
        EfCoreDatabaseProviderHelper.GetDatabaseProviderOrNull(context.Database.ProviderName)
            .ShouldBe(EfCoreDatabaseProvider.PostgreSql);
    }

    [Fact]
    public void Should_Detect_MySql_Pomelo_From_Assembly_Name()
    {
        var providerName = typeof(Microsoft.EntityFrameworkCore.MySqlDbContextOptionsBuilderExtensions).Assembly.GetName().Name;
        EfCoreDatabaseProviderHelper.GetDatabaseProviderOrNull(providerName)
            .ShouldBe(EfCoreDatabaseProvider.MySql);
    }

    [Fact]
    public void Should_Detect_MySql_Oracle_From_Real_Assembly()
    {
        var builder = new DbContextOptionsBuilder<EmptyDbContext>();
        Microsoft.EntityFrameworkCore.MySQLDbContextOptionsExtensions.UseMySQL(builder, "Server=localhost;Database=test");
        using var context = new EmptyDbContext(builder.Options);
        EfCoreDatabaseProviderHelper.GetDatabaseProviderOrNull(context.Database.ProviderName)
            .ShouldBe(EfCoreDatabaseProvider.MySql);
    }

    [Fact]
    public void Should_Detect_Oracle_From_Real_Assembly()
    {
        var builder = new DbContextOptionsBuilder<EmptyDbContext>();
        Microsoft.EntityFrameworkCore.OracleDbContextOptionsExtensions.UseOracle(builder, "Data Source=localhost/XE");
        using var context = new EmptyDbContext(builder.Options);
        EfCoreDatabaseProviderHelper.GetDatabaseProviderOrNull(context.Database.ProviderName)
            .ShouldBe(EfCoreDatabaseProvider.Oracle);
    }

    [Fact]
    public void Should_Detect_Sqlite_From_Real_Assembly()
    {
        var builder = new DbContextOptionsBuilder<EmptyDbContext>();
        Microsoft.EntityFrameworkCore.SqliteDbContextOptionsBuilderExtensions.UseSqlite(builder, "Data Source=:memory:");
        using var context = new EmptyDbContext(builder.Options);
        EfCoreDatabaseProviderHelper.GetDatabaseProviderOrNull(context.Database.ProviderName)
            .ShouldBe(EfCoreDatabaseProvider.Sqlite);
    }

    [Fact]
    public void Should_Detect_InMemory_From_Real_Assembly()
    {
        var builder = new DbContextOptionsBuilder<EmptyDbContext>();
        Microsoft.EntityFrameworkCore.InMemoryDbContextOptionsExtensions.UseInMemoryDatabase(builder, "test");
        using var context = new EmptyDbContext(builder.Options);
        EfCoreDatabaseProviderHelper.GetDatabaseProviderOrNull(context.Database.ProviderName)
            .ShouldBe(EfCoreDatabaseProvider.InMemory);
    }

    [Theory]
    [InlineData("Devart.Data.Oracle.Entity.EFCore", EfCoreDatabaseProvider.Oracle)]
    [InlineData("FirebirdSql.EntityFrameworkCore.Firebird", EfCoreDatabaseProvider.Firebird)]
    [InlineData("Microsoft.EntityFrameworkCore.Cosmos", EfCoreDatabaseProvider.Cosmos)]
    public void Should_Detect_Providers_Without_Package_Reference(string providerName, EfCoreDatabaseProvider expected)
    {
        EfCoreDatabaseProviderHelper.GetDatabaseProviderOrNull(providerName).ShouldBe(expected);
    }

    [Theory]
    [InlineData("microsoft.entityframeworkcore.sqlserver", EfCoreDatabaseProvider.SqlServer)]
    [InlineData("POMELO.ENTITYFRAMEWORKCORE.MYSQL", EfCoreDatabaseProvider.MySql)]
    [InlineData("npgsql.entityframeworkcore.postgresql", EfCoreDatabaseProvider.PostgreSql)]
    public void Should_Detect_Providers_Case_Insensitively(string providerName, EfCoreDatabaseProvider expected)
    {
        EfCoreDatabaseProviderHelper.GetDatabaseProviderOrNull(providerName).ShouldBe(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Some.Unknown.Provider")]
    public void Should_Return_Null_For_Unknown_Or_Empty(string? providerName)
    {
        EfCoreDatabaseProviderHelper.GetDatabaseProviderOrNull(providerName).ShouldBeNull();
    }

    private class EmptyDbContext : DbContext
    {
        public EmptyDbContext(DbContextOptions<EmptyDbContext> options) : base(options)
        {
        }
    }
}
