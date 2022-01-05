using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Shouldly;
using Volo.Abp.IdentityServer.ApiResources;
using Xunit;

namespace Volo.Abp.IdentityServer;

public class ResourceStore_Cache_Tests : AbpIdentityServerDomainTestBase
{
    private readonly IResourceStore _resourceStore;

    public ResourceStore_Cache_Tests()
    {
        _resourceStore = GetRequiredService<IResourceStore>();
    }

    [Fact]
    public async Task FindIdentityResourcesByScopeNameAsync()
    {
        var identityResources = (await _resourceStore.FindIdentityResourcesByScopeNameAsync(new[] { "Test-Identity-Resource-Name-1" })).ToList();
        identityResources.ShouldNotBeEmpty();
        identityResources.Count.ShouldBe(1);
        identityResources.First().Name.ShouldBe("Test-Identity-Resource-Name-1");

        var identityResources2 = (await _resourceStore.FindIdentityResourcesByScopeNameAsync(new[] { "Test-Identity-Resource-Name-1", "NewIdentityResource1" })).ToList();
        identityResources2.ShouldNotBeEmpty();
        identityResources2.Count.ShouldBe(2);
        identityResources2.ShouldContain(x => x.Name == "NewIdentityResource1");
        identityResources2.ShouldContain(x => x.Name == identityResources.First().Name);
    }

    [Fact]
    public async Task FindApiScopesByNameAsync()
    {
        var apiScopes1 = (await _resourceStore.FindApiScopesByNameAsync(new[] { "Test-ApiScope-Name-1" })).ToList();
        apiScopes1.ShouldNotBeEmpty();
        apiScopes1.Count.ShouldBe(1);
        apiScopes1.First().Name.ShouldBe("Test-ApiScope-Name-1");

        var apiScopes2 = (await _resourceStore.FindApiScopesByNameAsync(new[] { "Test-ApiScope-Name-1", "Test-ApiScope-Name-2" })).ToList();
        apiScopes2.ShouldNotBeEmpty();
        apiScopes2.Count.ShouldBe(2);
        apiScopes2.ShouldContain(x => x.Name == "Test-ApiScope-Name-1");
        apiScopes2.ShouldContain(x => x.Name == apiScopes1.First().Name);
    }

    [Fact]
    public async Task FindApiResourcesByScopeNameAsync()
    {
        var apiResources1 = (await _resourceStore.FindApiResourcesByScopeNameAsync(new[] { "Test-ApiResource-ApiScope-Name-1" })).ToList();
        apiResources1.ShouldNotBeEmpty();
        apiResources1.Count.ShouldBe(1);
        apiResources1.First().Name.ShouldBe("Test-ApiResource-Name-1");

        var apiResources2 = (await _resourceStore.FindApiResourcesByScopeNameAsync(new[] { "Test-ApiResource-ApiScope-Name-1", nameof(ApiResourceScope.Scope) })).ToList();
        apiResources2.ShouldNotBeEmpty();
        apiResources2.Count.ShouldBe(2);
        apiResources2.ShouldContain(x => x.Name == "Test-ApiResource-Name-1");
        apiResources2.ShouldContain(x => x.Name == apiResources1.First().Name);
    }

    [Fact]
    public async Task FindApiResourcesByNameAsync()
    {
        var apiResources1 = (await _resourceStore.FindApiResourcesByNameAsync(new[] { "Test-ApiResource-Name-1" })).ToList();
        apiResources1.ShouldNotBeEmpty();
        apiResources1.Count.ShouldBe(1);
        apiResources1.First().Name.ShouldBe("Test-ApiResource-Name-1");

        var apiResources2 = (await _resourceStore.FindApiResourcesByNameAsync(new[] { "Test-ApiResource-Name-1", "NewApiResource1" })).ToList();
        apiResources2.ShouldNotBeEmpty();
        apiResources2.Count.ShouldBe(2);
        apiResources2.ShouldContain(x => x.Name == "NewApiResource1");
        apiResources2.ShouldContain(x => x.Name == apiResources1.First().Name);
    }
}
