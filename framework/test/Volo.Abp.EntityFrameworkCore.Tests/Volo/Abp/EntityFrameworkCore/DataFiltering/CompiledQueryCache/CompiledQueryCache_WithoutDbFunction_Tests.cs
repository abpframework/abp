using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCache_WithoutDbFunction_Tests : CompiledQueryCacheTestBase<CompiledQueryCacheTestModuleWithoutDbFunction>
{
    protected override bool UseDbFunction => false;

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

    [Fact]
    public async Task Should_Read_Filter_State_On_Every_Run_Of_Compiled_Query()
    {
        var query = EF.CompileQuery((CompiledQueryCacheTestDbContext db) =>
            db.Rows.OrderBy(x => x.Name).Select(x => x.Name));
        var dataFilter = GetRequiredService<IDataFilter>();

        (await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(query(db).ToList())))
            .ShouldBe(new[] { "A-live" });

        using (dataFilter.Disable<ISoftDelete>())
        {
            (await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(query(db).ToList())))
                .ShouldBe(new[] { "A-deleted", "A-live" });
        }

        using (dataFilter.Disable<IMultiTenant>())
        {
            (await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(query(db).ToList())))
                .ShouldBe(new[] { "A-live", "B-live", "Host-live" });
        }

        (await RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db => Task.FromResult(query(db).ToList())))
            .ShouldBe(new[] { "A-live" });
    }
}
