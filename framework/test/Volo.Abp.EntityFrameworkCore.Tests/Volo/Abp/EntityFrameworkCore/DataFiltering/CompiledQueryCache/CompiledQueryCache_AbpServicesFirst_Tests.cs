using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TestApp.Testing;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

public class CompiledQueryCache_AbpServicesFirst_Tests : TestAppTestBase<CompiledQueryCacheTestModuleWithAbpServicesFirst>
{
    [Fact]
    public async Task Should_Not_Run_Query_Without_Filters()
    {
        using var uow = GetRequiredService<IUnitOfWorkManager>().Begin();
        using var tenant = GetRequiredService<ICurrentTenant>().Change(CompiledQueryCacheTestModuleBase.TenantA);
        var context = GetRequiredService<CompiledQueryCacheTestDbContext>();
        context.UseDbFunction().ShouldBeTrue();

        var exception = await Should.ThrowAsync<AbpException>(() => context.Rows.Select(row => row.Name).ToListAsync());
        exception.Message.ShouldContain("AbpDbContextOptionsExtension");
    }
}
