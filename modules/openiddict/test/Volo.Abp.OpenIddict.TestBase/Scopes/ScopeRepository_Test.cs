using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Scopes;
using Xunit;

namespace Volo.Abp.OpenIddict.TestBase.Scopes;
public abstract class ScopeRepository_Test<TStartupModule> : OpenIddictTestBase<TStartupModule>
       where TStartupModule : IAbpModule
{
    protected IOpenIddictScopeRepository ScopeRepository { get; }

    protected ScopeRepository_Test()
    {
        ScopeRepository = ServiceProvider.GetRequiredService<IOpenIddictScopeRepository>();
    }

    [Fact]
    public async Task GetListAsync_ShouldReturnAllOfThem()
    {
        var list = await ScopeRepository.GetListAsync();

        list.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetListAsync_ShouldFilterCorrectly()
    {
        var list = await ScopeRepository.GetListAsync(
            skipCount: 0,
            maxResultCount: 10,
            sorting: "Id",
            filter: AbpOpenIddictTestData.Scope1Name);

        list.ShouldNotBeEmpty();
        list.Count.ShouldBe(1);
    }
}
