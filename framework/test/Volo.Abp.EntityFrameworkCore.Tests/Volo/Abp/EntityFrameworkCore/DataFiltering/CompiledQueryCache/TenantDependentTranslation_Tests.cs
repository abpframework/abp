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
using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class TenantDependentTranslation_Tests : TestAppTestBase<TenantDependentTranslationTestModule>
{
    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task Should_Isolate_Tenants_When_Custom_Translation_Includes_Tenant_In_Cache_Key(
        bool asyncQuery,
        bool hostFirst)
    {
        var queryTag = Guid.NewGuid().ToString();
        var counter = GetRequiredService<QueryCompilationCounter>();
        var before = counter.Count;
        IModel sharedModel = null;
        var cacheKeys = new HashSet<object>();
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
                CompiledQueryCacheTestModuleBase.TenantA,
                null,
                CompiledQueryCacheTestModuleBase.TenantB
            };

        foreach (var tenantId in tenants)
        {
            using var scope = ServiceProvider.CreateScope();
            using var tenant = scope.ServiceProvider.GetRequiredService<ICurrentTenant>().Change(tenantId);
            using var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin();
            var context = scope.ServiceProvider.GetRequiredService<TenantDependentTranslationTestDbContext>();
            context.UseDbFunction().ShouldBeTrue();
            context.CurrentTenant.Id.ShouldBe(tenantId);

            sharedModel ??= context.Model;
            context.Model.ShouldBeSameAs(sharedModel);

            var query = context.Rows
                .AsNoTracking()
                .TagWith(queryTag)
                .OrderBy(row => row.Name)
                .Select(row => row.Name);
            var cacheKeyGenerator = context.GetService<ICompiledQueryCacheKeyGenerator>();
            cacheKeyGenerator.ShouldBeOfType<AbpCompiledQueryCacheKeyGenerator>();
            var cacheKey = cacheKeyGenerator.GenerateCacheKey(query.Expression, asyncQuery);
            cacheKeys.Add(cacheKey);
            var names = asyncQuery ? await query.ToListAsync() : query.ToList();
            (counter.Count - before).ShouldBe(cacheKeys.Count);

            names.ShouldBe(TenantDependentTranslationTestModule.CustomRows
                .Where(row => row.OwnerTenantId == tenantId)
                .Select(row => row.Name)
                .OrderBy(name => name)
                .ToArray());
            await uow.CompleteAsync();
        }

        cacheKeys.Count.ShouldBe(3);
    }
}
