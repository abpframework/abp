using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Scopes;
using Xunit;

namespace Volo.Abp.OpenIddict;

public abstract class OpenIddictScopeRepository_Tests<TStartupModule> : OpenIddictTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IOpenIddictScopeRepository _scopeRepository;
    private readonly AbpOpenIddictTestData _testData;

    public OpenIddictScopeRepository_Tests()
    {
        _scopeRepository = GetRequiredService<IOpenIddictScopeRepository>();
        _testData = GetRequiredService<AbpOpenIddictTestData>();
    }

    [Fact]
    public async Task GetListAsync()
    {
        (await _scopeRepository.GetListAsync("id", 0, int.MaxValue)).Count.ShouldBe(2);
        (await _scopeRepository.GetListAsync("id", 0, int.MaxValue, filter: _testData.Scope1Name)).Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetCountAsync()
    {
        (await _scopeRepository.GetCountAsync()).ShouldBe(2);
        (await _scopeRepository.GetCountAsync(filter: _testData.Scope1Name)).ShouldBe(1);
    }

    [Fact]
    public async Task FindByIdAsync()
    {
        var scope = await _scopeRepository.FindByIdAsync(_testData.Scope1Id);

        scope.ShouldNotBeNull();
        scope.Id.ShouldBe(_testData.Scope1Id);
    }

    [Fact]
    public async Task FindByNameAsync()
    {
        var scope = await _scopeRepository.FindByNameAsync(_testData.Scope1Name);

        scope.ShouldNotBeNull();
        scope.Name.ShouldBe(_testData.Scope1Name);
    }

    [Fact]
    public async Task FindByNamesAsync()
    {
        (await _scopeRepository.FindByNamesAsync(new []{_testData.Scope1Name, _testData.Scope2Name})).Count.ShouldBe(2);
    }

    [Fact]
    public async Task FindByResourceAsync()
    {
        (await _scopeRepository.FindByResourceAsync("TestScope1Resource")).Count.ShouldBe(1);
    }

    [Fact]
    public async Task ListAsync()
    {
        (await _scopeRepository.ListAsync(int.MaxValue, 0)).Count.ShouldBe(2);
        (await _scopeRepository.ListAsync(int.MaxValue, 2)).Count.ShouldBe(0);
    }
}