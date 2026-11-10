using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCache_DatabaseFactory_Tests : CompiledQueryCacheTestBase<CompiledQueryCacheTestModuleWithDatabaseFactory>
{
    protected override bool UseDbFunction => true;

    [Fact]
    public async Task Should_Not_Run_Compiled_Query_First_Run_Inside_Another_Query_Without_Filters()
    {
        var countRows = EF.CompileQuery((CompiledQueryCacheTestDbContext db) => db.Rows.Count());

        var exception = await Should.ThrowAsync<InvalidOperationException>(() =>
            RunInTenantAsync(CompiledQueryCacheTestModuleBase.TenantA, db =>
                Task.FromResult(db.Rows.Where(row => row.Name.Length > countRows(db)).Select(row => row.Name).ToList())));
        exception.InnerException.ShouldBeOfType<AbpException>();
    }

    [Fact]
    public async Task Should_Keep_Using_And_Disposing_The_Custom_Database()
    {
        var recorder = GetRequiredService<CompiledQueryCacheTestDatabaseRecorder>();
        var disposeCount = recorder.DisposeCount;

        using (var scope = ServiceProvider.CreateScope())
        {
            using var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin();
            using var tenant = scope.ServiceProvider.GetRequiredService<ICurrentTenant>().Change(CompiledQueryCacheTestModuleBase.TenantA);
            var db = scope.ServiceProvider.GetRequiredService<CompiledQueryCacheTestDbContext>();

            var row = await db.Rows.SingleAsync();
            row.Name = "A-saved";
            await db.SaveChangesAsync();
            await uow.CompleteAsync();
        }

        recorder.SaveChangesCount.ShouldBe(1);
        recorder.DisposeCount.ShouldBeGreaterThan(disposeCount);
    }
}
