using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.MySQL;

public class MySQLGuidArrayTypeMappingSourcePlugin_Tests
{
    /* Runs without a MySQL server: type mapping lookup is metadata-only, no
     * connection is opened. When upgrading the provider, verify its native
     * Guid[] mapping without the plugin registered; once the provider handles
     * Guid[] itself, remove MySQLGuidArrayTypeMappingSourcePlugin together
     * with this test. */
    [Fact]
    public void UseMySQL_Should_Map_Guid_Array_Parameter_To_A_Collection_Mapping()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        var configurationContext = new AbpDbContextConfigurationContext(
            "Server=localhost;Database=_;Uid=_;Pwd=_;",
            services.BuildServiceProvider(),
            null,
            null);
        configurationContext.UseMySQL();

        using var dbContext = new PluginTestDbContext(configurationContext.DbContextOptions.Options);
        var typeMappingSource = dbContext.GetService<IRelationalTypeMappingSource>();
        var elementMapping = typeMappingSource.FindMapping(typeof(Guid))!;

        var mapping = typeMappingSource.FindMapping(typeof(Guid[]), dbContext.Model, elementMapping);

        mapping.ShouldNotBeNull();
        mapping.ClrType.ShouldBe(typeof(Guid[]));
        mapping.StoreType.ShouldBe("longtext");
        mapping.ElementTypeMapping.ShouldBe(elementMapping);
    }

    private class PluginTestDbContext : DbContext
    {
        public PluginTestDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<PluginTestEntity> Entities => Set<PluginTestEntity>();
    }

    private class PluginTestEntity
    {
        public Guid Id { get; set; }
    }
}
