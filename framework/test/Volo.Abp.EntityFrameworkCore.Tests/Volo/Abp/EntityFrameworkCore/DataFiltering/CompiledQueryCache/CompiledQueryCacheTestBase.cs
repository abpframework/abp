using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public abstract class CompiledQueryCacheTestBase<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly string _queryTag = Guid.NewGuid().ToString();

    protected abstract bool UseDbFunction { get; }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task Should_Reuse_Compiled_Query_Without_Reusing_Tenant_Or_Filter_State(bool asyncQuery, bool hostFirst)
    {
        var counter = GetRequiredService<QueryCompilationCounter>();
        var before = counter.Count;
        IModel sharedModel = null;
        var cacheKeys = new Dictionary<(bool SoftDelete, bool MultiTenant), object>();
        var tenants = hostFirst
            ? new Guid?[]
            {
                null,
                CompiledQueryCacheTestModuleBase.TenantA,
                CompiledQueryCacheTestModuleBase.TenantB,
                null,
                CompiledQueryCacheTestModuleBase.TenantA
            }
            : new Guid?[]
            {
                CompiledQueryCacheTestModuleBase.TenantA,
                CompiledQueryCacheTestModuleBase.TenantB,
                null,
                CompiledQueryCacheTestModuleBase.TenantA
            };

        foreach (var tenantId in tenants)
        {
            foreach (var softDeleteEnabled in new[] { true, false, true })
            {
                foreach (var multiTenantEnabled in new[] { true, false, true })
                {
                    using var scope = ServiceProvider.CreateScope();
                    using var tenant = scope.ServiceProvider.GetRequiredService<ICurrentTenant>().Change(tenantId);
                    var dataFilter = scope.ServiceProvider.GetRequiredService<IDataFilter>();
                    using var softDelete = softDeleteEnabled
                        ? dataFilter.Enable<ISoftDelete>()
                        : dataFilter.Disable<ISoftDelete>();
                    using var multiTenant = multiTenantEnabled
                        ? dataFilter.Enable<IMultiTenant>()
                        : dataFilter.Disable<IMultiTenant>();
                    using var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin();
                    var context = scope.ServiceProvider.GetRequiredService<CompiledQueryCacheTestDbContext>();
                    context.UseDbFunction().ShouldBe(UseDbFunction);

                    sharedModel ??= context.Model;
                    context.Model.ShouldBeSameAs(sharedModel);

                    var query = context.Rows
                        .AsNoTracking()
                        .TagWith(_queryTag)
                        .OrderBy(x => x.Name)
                        .Select(x => x.Name);
                    var cacheKeyGenerator = context.GetService<ICompiledQueryCacheKeyGenerator>();
                    if (UseDbFunction)
                    {
                        cacheKeyGenerator.ShouldBeOfType<AbpCompiledQueryCacheKeyGenerator>();
                    }
                    else
                    {
                        cacheKeyGenerator.ShouldNotBeOfType<AbpCompiledQueryCacheKeyGenerator>();
                    }

                    var cacheKey = cacheKeyGenerator.GenerateCacheKey(query.Expression, asyncQuery);
                    var filterState = (softDeleteEnabled, multiTenantEnabled);
                    if (cacheKeys.TryGetValue(filterState, out var previousKey))
                    {
                        cacheKey.ShouldBe(previousKey);
                    }
                    else
                    {
                        cacheKeys.Add(filterState, cacheKey);
                    }

                    var names = asyncQuery ? await query.ToListAsync() : query.ToList();
                    names.ShouldBe(CompiledQueryCacheTestModuleBase.Rows
                        .Where(row => !softDeleteEnabled || !row.Deleted)
                        .Where(row => !multiTenantEnabled || row.TenantId == tenantId)
                        .Select(row => row.Name)
                        .OrderBy(name => name)
                        .ToArray());
                    await uow.CompleteAsync();
                }
            }
        }

        var expectedCompilationCount = UseDbFunction ? 4 : 1;
        cacheKeys.Values.Distinct().Count().ShouldBe(expectedCompilationCount);
        (counter.Count - before).ShouldBe(expectedCompilationCount);
    }

    [Fact]
    public async Task Should_Read_Current_Tenant_When_Reusing_Query_In_Same_DbContext()
    {
        using var uow = GetRequiredService<IUnitOfWorkManager>().Begin();
        var context = GetRequiredService<CompiledQueryCacheTestDbContext>();
        context.UseDbFunction().ShouldBe(UseDbFunction);
        var currentTenant = GetRequiredService<ICurrentTenant>();
        var counter = GetRequiredService<QueryCompilationCounter>();
        var before = counter.Count;
        var query = context.Rows
            .AsNoTracking()
            .TagWith(_queryTag)
            .OrderBy(x => x.Name)
            .Select(x => x.Name);
        var tenants = new Guid?[]
        {
            CompiledQueryCacheTestModuleBase.TenantA,
            CompiledQueryCacheTestModuleBase.TenantB,
            null,
            CompiledQueryCacheTestModuleBase.TenantA
        };

        foreach (var tenantId in tenants)
        {
            using var tenant = currentTenant.Change(tenantId);
            var names = await query.ToListAsync();
            names.ShouldBe(CompiledQueryCacheTestModuleBase.Rows
                .Where(row => !row.Deleted && row.TenantId == tenantId)
                .Select(row => row.Name)
                .OrderBy(name => name)
                .ToArray());
        }

        (counter.Count - before).ShouldBe(1);
        await uow.CompleteAsync();
    }

    [Fact]
    public async Task Should_Apply_Filters_To_Other_Query_Entry_Points()
    {
        var names = await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, async db =>
        {
            (await db.Rows.IgnoreQueryFilters().CountAsync()).ShouldBe(CompiledQueryCacheTestModuleBase.Rows.Length);
            db.Rows.Select(row => row.Name).ToQueryString().ShouldContain("IsDeleted");

            (await db.Rows.Where(row => row.Name == "B-live").ExecuteUpdateAsync(s => s.SetProperty(row => row.Name, "B-updated"))).ShouldBe(0);
            (await db.Rows.Where(row => row.Name == "A-live").ExecuteUpdateAsync(s => s.SetProperty(row => row.Name, "A-updated"))).ShouldBe(1);

            var row = await db.Rows.SingleAsync();
            row.Name = "A-saved";
            await db.SaveChangesAsync();

            return await db.Rows.AsNoTracking().Select(x => x.Name).ToListAsync();
        });

        names.ShouldBe(new[] { "A-saved" });
    }

    protected async Task<List<string>> RunInTenantAsync(
        Guid? tenantId,
        Func<CompiledQueryCacheTestDbContext, Task<List<string>>> query)
    {
        using var uow = GetRequiredService<IUnitOfWorkManager>().Begin();
        using var tenant = GetRequiredService<ICurrentTenant>().Change(tenantId);
        var context = GetRequiredService<CompiledQueryCacheTestDbContext>();
        context.UseDbFunction().ShouldBe(UseDbFunction);

        var names = await query(context);
        await uow.CompleteAsync();
        return names;
    }

    protected static async Task<List<string>> ToListAsync(IAsyncEnumerable<string> source)
    {
        var names = new List<string>();
        await foreach (var name in source)
        {
            names.Add(name);
        }

        return names;
    }
}
