﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.IdentityServer.Clients
{
    public class IdentityResourceStore_Tests : AbpIdentityServerTestBase
    {
        private readonly IResourceStore _resourceStore;

        public IdentityResourceStore_Tests()
        {
            _resourceStore = ServiceProvider.GetRequiredService<IResourceStore>();
        }

        [Fact]
        public async Task FindApiResourceAsync_Should_Return_Null_If_Not_Found()
        {
            //Act
            var resource = await _resourceStore.FindApiResourceAsync("non-existing-name").ConfigureAwait(false);

            //Assert
            resource.ShouldBeNull();
        }

        [Fact]
        public async Task FindApiResourceAsync_Should_Return_If_Found()
        {
            //Act
            var apiResource = await _resourceStore.FindApiResourceAsync("Test-ApiResource-Name-1").ConfigureAwait(false);

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
            var apiResources = (await _resourceStore.FindApiResourcesByScopeAsync(new List<string>
            {
                "Test-ApiResource-ApiScope-Name-1"
            }).ConfigureAwait(false)).ToList();

            //Assert
            apiResources.ShouldNotBe(null);

            apiResources[0].Scopes.Count.ShouldBe(2);
        }

        [Fact]
        public async Task FindIdentityResourcesByScopeAsync_Should_Return_For_Given_Scopes()
        {
            //Act
            var identityResourcesByScope = await _resourceStore.FindIdentityResourcesByScopeAsync(new List<string>
            {
                "Test-Identity-Resource-Name-1"
            }).ConfigureAwait(false);

            //Assert
            var resourcesByScope = identityResourcesByScope as IdentityResource[] ?? identityResourcesByScope.ToArray();
            resourcesByScope.Length.ShouldBe(1);
            resourcesByScope.First().DisplayName.ShouldBe("Test-Identity-Resource-DisplayName-1");
            resourcesByScope.First().Description.ShouldBe("Test-Identity-Resource-Description-1");
            resourcesByScope.First().Required.ShouldBe(true);
        }

        [Fact]
        public async Task GetAllResourcesAsync_Should_Return()
        {
            //Act
            var resources = await _resourceStore.GetAllResourcesAsync().ConfigureAwait(false);

            //Assert
            resources.ShouldNotBe(null);
            resources.ApiResources.Count.ShouldBeGreaterThan(0);
            resources.ApiResources.Any(r => r.Name == "Test-ApiResource-Name-1").ShouldBeTrue();
        }
    }
}
