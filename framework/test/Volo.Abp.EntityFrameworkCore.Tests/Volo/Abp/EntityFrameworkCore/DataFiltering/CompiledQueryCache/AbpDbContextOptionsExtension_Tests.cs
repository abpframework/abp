using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class AbpDbContextOptionsExtension_Tests
{
    [Fact]
    public void Should_Create_And_Delete_InMemory_Database()
    {
        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .AddAbpDbContextOptionsExtension()
            .Options;

        using var context = new DbContext(options);
        context.GetService<IDatabase>().ShouldNotBeOfType<AbpEfCoreDatabase>();
        context.Database.EnsureCreated().ShouldBeTrue();
        context.Database.EnsureDeleted().ShouldBeTrue();
    }

    [Fact]
    public void Should_Apply_Services_Once()
    {
        var services = new ServiceCollection();
        services.AddEntityFrameworkSqlite();

        new AbpDbContextOptionsExtension().ApplyServices(services);
        new AbpDbContextOptionsExtension().ApplyServices(services);

        services.Count(x => x.ServiceType == typeof(IDatabase) && x.IsKeyedService).ShouldBe(1);
        services.Count(x => x.ServiceType == typeof(AbpEfCoreCurrentDbContext)).ShouldBe(1);
    }

    [Fact]
    public void Should_Apply_Services_Again_After_The_Database_Provider()
    {
        var services = new ServiceCollection();
        var extension = new AbpDbContextOptionsExtension();

        extension.ApplyServices(services);
        services.AddEntityFrameworkSqlite();
        extension.ApplyServices(services);

        services.Count(x => x.ServiceType == typeof(IDatabase) && x.IsKeyedService).ShouldBe(1);
        services.Count(x => x.ServiceType == typeof(ICompiledQueryCacheKeyGenerator) && x.IsKeyedService).ShouldBe(1);
        services.Count(x => x.ServiceType == typeof(AbpEfCoreCurrentDbContext)).ShouldBe(1);
    }

    [Fact]
    public void Should_Decorate_Database_Registered_After_Applying_Services()
    {
        var services = new ServiceCollection();
        var extension = new AbpDbContextOptionsExtension();
        services.AddEntityFrameworkSqlite();
        extension.ApplyServices(services);
        services.AddScoped<IDatabase>(provider => ActivatorUtilities.CreateInstance<RelationalDatabase>(provider));
        extension.ApplyServices(services);

        using var serviceProvider = services.BuildServiceProvider();
        var options = new DbContextOptionsBuilder()
            .UseSqlite("Data Source=:memory:")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        using var context = new DbContext(options);
        context.GetService<IDatabase>().ShouldBeOfType<AbpEfCoreDatabase>();
    }
}
