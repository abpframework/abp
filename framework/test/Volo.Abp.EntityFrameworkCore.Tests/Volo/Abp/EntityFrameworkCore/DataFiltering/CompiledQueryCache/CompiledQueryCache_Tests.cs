using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCache_Tests : CompiledQueryCacheTestBase<CompiledQueryCacheTestModule>
{
    protected override bool UseDbFunction => true;

    // EF.CompileQuery translates the query outside of AbpEntityQueryProvider, so the filter state is not available.
    // Each filter is tested on its own entity, so one translation cannot hide a missing check in the other.
    [Fact]
    public async Task Should_Not_Run_Compiled_Query_Without_SoftDelete_Filter()
    {
        var query = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.SoftDeleteRows.Select(x => x.Name));

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(query(db).ToList())));
    }

    [Fact]
    public async Task Should_Not_Run_Compiled_Query_Without_MultiTenant_Filter()
    {
        var query = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.MultiTenantRows.Select(x => x.Name));

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(query(db).ToList())));
    }

    // The compiled query is first run while the outer query is executed, so the current DbContext of the outer query is set.
    [Fact]
    public async Task Should_Not_Run_Compiled_Query_First_Run_Inside_Another_Query_Without_Filters()
    {
        var countRows = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());

        // EF Core wraps the exceptions thrown while it evaluates the parameters of the outer query.
        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
                Task.FromResult(db.Rows.Where(row => row.Name.Length > countRows(db)).Select(row => row.Name).ToList())));
        exception.InnerException.ShouldBeOfType<AbpException>();

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(new[] { countRows(db).ToString() }.ToList())));
    }

    [Fact]
    public async Task Should_Not_Run_Compiled_Async_Query_Without_Filters()
    {
        var query = EF.CompileAsyncQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Select(x => x.Name));

        await Should.ThrowAsync<AbpException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => ToListAsync(query(db))));
    }
}
