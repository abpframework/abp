using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.Testing;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

#pragma warning disable EF1001
public class CompiledQueryCache_QueryCompiler_Tests : TestAppTestBase<CompiledQueryCacheTestModuleWithQueryCompiler>
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Should_Use_The_Replaced_Query_Compiler(bool replaceImplementationType)
    {
        GetRequiredService<CompiledQueryCacheTestQueryCompilerOptions>().ReplaceImplementationType = replaceImplementationType;

        await WithUnitOfWorkAsync(async () =>
        {
            var currentTenant = GetRequiredService<ICurrentTenant>();
            using (currentTenant.Change(CompiledQueryCacheTestModuleBase.TenantA))
            {
                var db = GetRequiredService<CompiledQueryCacheTestDbContext>();
                db.UseDbFunction().ShouldBeTrue();
                var queryCompiler = db.GetService<IQueryCompiler>().ShouldBeOfType<CompiledQueryCacheTestQueryCompiler>();

                (await db.Rows.Select(row => row.Name).ToListAsync()).ShouldBe(new[] { "A-live" });
                using (currentTenant.Change(CompiledQueryCacheTestModuleBase.TenantB))
                {
                    db.Rows.Select(row => row.Name).ToList().ShouldBe(new[] { "B-live" });
                }

                queryCompiler.ExecuteCount.ShouldBe(2);
            }
        });
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Should_Not_Run_Compiled_Query_First_Run_Inside_Another_Query_Without_Filters(bool replaceImplementationType)
    {
        GetRequiredService<CompiledQueryCacheTestQueryCompilerOptions>().ReplaceImplementationType = replaceImplementationType;
        var countRows = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());

        await WithUnitOfWorkAsync(async () =>
        {
            using (GetRequiredService<ICurrentTenant>().Change(CompiledQueryCacheTestModuleBase.TenantA))
            {
                var db = GetRequiredService<CompiledQueryCacheTestDbContext>();
                var queryCompiler = db.GetService<IQueryCompiler>().ShouldBeOfType<CompiledQueryCacheTestQueryCompiler>();
                var dataFilter = GetRequiredService<IDataFilter>();

                using (dataFilter.Disable<ISoftDelete>())
                using (dataFilter.Disable<IMultiTenant>())
                {
                    var exception = Should.Throw<InvalidOperationException>(() =>
                        db.Rows.Where(row => row.Name.Length > countRows(db)).Select(row => row.Name).ToList());
                    exception.InnerException.ShouldBeOfType<AbpException>();
                }

                queryCompiler.CreateCompiledQueryCount.ShouldBe(1);
                (await db.Rows.Select(row => row.Name).ToListAsync()).ShouldBe(new[] { "A-live" });
            }
        });
    }
}
#pragma warning restore EF1001
