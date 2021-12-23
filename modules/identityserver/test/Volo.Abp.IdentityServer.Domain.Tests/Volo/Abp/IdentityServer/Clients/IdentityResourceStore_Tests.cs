using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.IdentityServer.Clients;

public class IdentityResourceStore_Tests : AbpIdentityServerTestBase
{
    private readonly IResourceStore _resourceStore;

    public IdentityResourceStore_Tests()
    {
        _resourceStore = ServiceProvider.GetRequiredService<IResourceStore>();
    }

    [Fact]
    public async Task FindApiResourceAsync_Should_Return_Empty_If_Not_Found()
    {
        //Act
        var resource = await _resourceStore.FindApiResourcesByNameAsync(new[] { "non-existing-name" });

        //Assert
        resource.ShouldBeEmpty();
    }

    [Fact]
    public async Task FindApiResourceAsync_Should_Return_If_Found()
    {
        //Act
        var apiResource = (await _resourceStore.FindApiResourcesByNameAsync(new[] { "Test-ApiResource-Name-1" })).FirstOrDefault();

        //Assert
        apiResource.ShouldNotBe(null);
        apiResource.Name.ShouldBe("Test-ApiResource-Name-1");
        apiResource.Description.ShouldBe("Test-ApiResource-Description-1");
        apiResource.DisplayName.ShouldBe("Test-ApiResource-DisplayName-1");
    }

    [Fact]
    public async Task FindApiResourcesByScopeAsync_Should_Return_If_Found()
    {
        //Act
        var apiResources = (await _resourceStore.FindApiResourcesByScopeNameAsync(new List<string>
            {
                "Test-ApiResource-ApiScope-Name-1"
            })).ToList();

        //Assert
        apiResources.ShouldNotBe(null);

        apiResources[0].Scopes.Count.ShouldBe(3);
    }

    [Fact]
    public async Task FindIdentityResourcesByScopeAsync_Should_Return_For_Given_Scopes()
    {
        //Act
        var identityResourcesByScope = (await _resourceStore.FindIdentityResourcesByScopeNameAsync(new List<string>
            {
                "Test-Identity-Resource-Name-1"
            })).ToArray();

        //Assert
        identityResourcesByScope.Length.ShouldBe(1);
        identityResourcesByScope.First().DisplayName.ShouldBe("Test-Identity-Resource-DisplayName-1");
        identityResourcesByScope.First().Description.ShouldBe("Test-Identity-Resource-Description-1");
        identityResourcesByScope.First().Required.ShouldBe(true);
    }

    [Fact]
    public async Task GetAllResourcesAsync_Should_Return()
    {
        //Act
        var resources = await _resourceStore.GetAllResourcesAsync();

        //Assert
        resources.ShouldNotBe(null);
        resources.ApiResources.Count.ShouldBeGreaterThan(0);
        resources.ApiResources.Any(r => r.Name == "Test-ApiResource-Name-1").ShouldBeTrue();
    }
}
