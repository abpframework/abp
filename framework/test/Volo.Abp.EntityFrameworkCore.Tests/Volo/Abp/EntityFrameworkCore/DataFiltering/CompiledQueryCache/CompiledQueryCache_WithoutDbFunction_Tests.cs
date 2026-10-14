using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCache_WithoutDbFunction_Tests : CompiledQueryCacheTestBase<CompiledQueryCacheTestModuleWithoutDbFunction>
{
    protected override bool UseDbFunction => false;

    // Without DbFunction the filters are parameters of the compiled query, so they are applied on every run.
    [Fact]
    public async Task Should_Apply_Filters_To_Compiled_Queries()
    {
        var query = EF.CompileQuery((CompiledQueryCacheTestDbContext db) =>
            db.Rows.OrderBy(x => x.Name).Select(x => x.Name));
        var asyncQuery = EF.CompileAsyncQuery((CompiledQueryCacheTestDbContext db) =>
            db.Rows.OrderBy(x => x.Name).Select(x => x.Name));

        (await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(query(db).ToList())))
            .ShouldBe(new[] { "A-live" });
        (await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantB, db => Task.FromResult(query(db).ToList())))
            .ShouldBe(new[] { "B-live" });
        (await RunInTenantAsync(null, db => ToListAsync(asyncQuery(db))))
            .ShouldBe(new[] { "Host-live" });
        (await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => ToListAsync(asyncQuery(db))))
            .ShouldBe(new[] { "A-live" });
    }
}
