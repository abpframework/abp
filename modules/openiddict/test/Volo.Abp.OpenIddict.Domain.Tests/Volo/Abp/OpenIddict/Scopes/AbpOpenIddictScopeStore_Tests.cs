using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Shouldly;
using Xunit;

namespace Volo.Abp.OpenIddict.Scopes;

public class AbpOpenIddictScopeStore_Tests : OpenIddictDomainTestBase
{
    private readonly IOpenIddictScopeStore<OpenIddictScopeModel> _scopeStore;
    private readonly AbpOpenIddictTestData _testData;

    public AbpOpenIddictScopeStore_Tests()
    {
        _scopeStore = ServiceProvider.GetRequiredService<IOpenIddictScopeStore<OpenIddictScopeModel>>();
        _testData = ServiceProvider.GetRequiredService<AbpOpenIddictTestData>();
    }

    [Fact]
    public async Task CountAsync()
    {
        var count = await _scopeStore.CountAsync(CancellationToken.None);
        count.ShouldBe(2);
    }

    [Fact]
    public async Task CreateAsync()
    {
        await _scopeStore.CreateAsync(
            new OpenIddictScopeModel {
                Name = "scope3", Description = "scope3 description", DisplayName = "scope3 display name"
            }, CancellationToken.None);

        var scope = await _scopeStore.FindByNameAsync("scope3", CancellationToken.None);

        scope.ShouldNotBeNull();
        scope.Name.ShouldBe("scope3");
        scope.Description.ShouldBe("scope3 description");
        scope.DisplayName.ShouldBe("scope3 display name");
        scope.Descriptions.ShouldBeNull();
        scope.DisplayNames.ShouldBeNull();
        scope.Resources.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        await _scopeStore.DeleteAsync(scope, CancellationToken.None);

        scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        scope.ShouldBeNull();
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Null_If_Not_Found()
    {
        var nonExistingId = Guid.NewGuid().ToString();
        var scope = await _scopeStore.FindByIdAsync(nonExistingId, CancellationToken.None);
        scope.ShouldBeNull();
    }

    [Fact]
    public async Task FindByIdAsync_Should_Return_Scope_If_Found()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);

        scope.ShouldNotBeNull();
        scope.Name.ShouldBe(_testData.Scope1Name);
        scope.DisplayName.ShouldBe("Test Scope 1");
        scope.Description.ShouldBe("Test Scope 1");
        scope.DisplayNames.ShouldContain("测试范围1");
        scope.DisplayNames.ShouldContain("Test Kapsamı 1");
        scope.Resources.ShouldBe("[\"TestScope1Resource\"]");
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Null_If_Not_Found()
    {
        var nonExistingName = Guid.NewGuid().ToString();
        var scope = await _scopeStore.FindByNameAsync(nonExistingName, CancellationToken.None);
        scope.ShouldBeNull();
    }

    [Fact]
    public async Task FindByNameAsync_Should_Return_Application_If_Found()
    {
        var scope = await _scopeStore.FindByNameAsync(_testData.Scope1Name, CancellationToken.None);

        scope.ShouldNotBeNull();
        scope.Name.ShouldBe(_testData.Scope1Name);
        scope.DisplayName.ShouldBe("Test Scope 1");
        scope.Description.ShouldBe("Test Scope 1");
        scope.DisplayNames.ShouldContain("测试范围1");
        scope.DisplayNames.ShouldContain("Test Kapsamı 1");
        scope.Resources.ShouldBe("[\"TestScope1Resource\"]");
    }

    [Fact]
    public async Task FindByNamesAsync_Should_Return_Empty_If_Not_Found()
    {
        var scopes = await _scopeStore
            .FindByNamesAsync(ImmutableArray.Create("non-existing-name"), CancellationToken.None).ToListAsync();
        scopes.Count.ShouldBe(0);
    }

    [Fact]
    public async Task FindByNamesAsync_Should_Return_Scopes_If_Found()
    {
        var scopes = await _scopeStore
            .FindByNamesAsync(ImmutableArray.Create("Scope1", "Scope2", "Scope3"), CancellationToken.None)
            .ToListAsync();
        scopes.Count.ShouldBe(2);
    }

    [Fact]
    public async Task FindByResourceAsync_Should_Return_Empty_If_Not_Found()
    {
        var scopes = await _scopeStore.FindByResourceAsync("non-existing-resource", CancellationToken.None)
            .ToListAsync();
        scopes.Count.ShouldBe(0);
    }

    [Fact]
    public async Task FindByResourceAsync_Should_Return_Scopes_If_Found()
    {
        var scopes = await _scopeStore.FindByResourceAsync("TestScope1Resource", CancellationToken.None).ToListAsync();
        scopes.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetDescriptionAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        var description = await _scopeStore.GetDescriptionAsync(scope, CancellationToken.None);
        description.ShouldBe("Test Scope 1");
    }

    [Fact]
    public async Task GetDescriptionsAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        var descriptions = await _scopeStore.GetDescriptionsAsync(scope, CancellationToken.None);
        descriptions.Count.ShouldBe(0);
    }

    [Fact]
    public async Task GetDisplayNameAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        var displayName = await _scopeStore.GetDisplayNameAsync(scope, CancellationToken.None);
        displayName.ShouldBe("Test Scope 1");
    }

    [Fact]
    public async Task GetDisplayNamesAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        var displayNames = await _scopeStore.GetDisplayNamesAsync(scope, CancellationToken.None);
        displayNames.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetIdAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        var id = await _scopeStore.GetIdAsync(scope, CancellationToken.None);
        id.ShouldBe(_testData.Scope1Id.ToString());
    }

    [Fact]
    public async Task GetNameAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        var name = await _scopeStore.GetNameAsync(scope, CancellationToken.None);
        name.ShouldBe("Scope1");
    }

    [Fact]
    public async Task GetPropertiesAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        var properties = await _scopeStore.GetPropertiesAsync(scope, CancellationToken.None);
        properties.Count.ShouldBe(0);
    }

    [Fact]
    public async Task GetResourcesAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        var resources = await _scopeStore.GetResourcesAsync(scope, CancellationToken.None);
        resources.Length.ShouldBe(1);
        resources.First().ShouldBe("TestScope1Resource");
    }

    [Fact]
    public async Task InstantiateAsync()
    {
        var scope = await _scopeStore.InstantiateAsync(CancellationToken.None);
        scope.ShouldNotBeNull();
    }

    [Fact]
    public async Task ListAsync_Should_Return_Empty_If_Not_Found()
    {
        var scopes = await _scopeStore.ListAsync(2, 2, CancellationToken.None).ToListAsync();
        scopes.Count.ShouldBe(0);
    }

    [Fact]
    public async Task ListAsync_Should_Return_Applications_If_Found()
    {
        var scopes = await _scopeStore.ListAsync(2, 0, CancellationToken.None).ToListAsync();

        scopes.Count.ShouldBe(2);
    }

    [Fact]
    public async Task SetDescriptionAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        await _scopeStore.SetDescriptionAsync(scope, "New Test Scope 1 Description", CancellationToken.None);

        scope.Description.ShouldBe("New Test Scope 1 Description");
    }

    [Fact]
    public async Task SetDescriptionsAsync()
    {
        var descriptions = ImmutableDictionary.Create<CultureInfo, string>();
        descriptions = descriptions.Add(CultureInfo.GetCultureInfo("en"), "Test Scope");
        descriptions = descriptions.Add(CultureInfo.GetCultureInfo("zh-Hans"), "测试范围");

        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        await _scopeStore.SetDescriptionsAsync(scope, descriptions, CancellationToken.None);

        scope.Descriptions.ShouldContain("Test Scope");
        scope.Descriptions.ShouldContain("测试范围");
    }

    [Fact]
    public async Task SetDisplayNameAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        await _scopeStore.SetDisplayNameAsync(scope, "New Test Scope 1 Display Name", CancellationToken.None);

        scope.DisplayName.ShouldBe("New Test Scope 1 Display Name");
    }

    [Fact]
    public async Task SetDisplayNamesAsync()
    {
        var displayNames = ImmutableDictionary.Create<CultureInfo, string>();
        displayNames = displayNames.Add(CultureInfo.GetCultureInfo("en"), "Test Scope");
        displayNames = displayNames.Add(CultureInfo.GetCultureInfo("zh-Hans"), "测试范围");

        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        await _scopeStore.SetDisplayNamesAsync(scope, displayNames, CancellationToken.None);

        scope.DisplayNames.ShouldContain("Test Scope");
        scope.DisplayNames.ShouldContain("测试范围");
    }

    [Fact]
    public async Task SetNameAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        await _scopeStore.SetNameAsync(scope, "New Test Scope 1 Name", CancellationToken.None);

        scope.Name.ShouldBe("New Test Scope 1 Name");
    }

    [Fact]
    public async Task SetPropertiesAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        await _scopeStore.SetPropertiesAsync(scope, ImmutableDictionary.Create<string, JsonElement>(),
            CancellationToken.None);

        scope.Properties.ShouldBeNull();
    }

    [Fact]
    public async Task SetResourcesAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        await _scopeStore.SetResourcesAsync(scope, ImmutableArray.Create("TestScope1Resource", "TestScope1Resource2"),
            CancellationToken.None);

        scope.Resources.ShouldContain("TestScope1Resource");
        scope.Resources.ShouldContain("TestScope1Resource2");
    }

    [Fact]
    public async Task UpdateAsync()
    {
        var scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        scope.Name = "New Test Scope 1 Name";
        scope.DisplayName = "New Test Scope 1 Display Name";
        scope.Resources = "New Test Scope 1 Resource";
        scope.Properties = "New Test Scope 1 Properties";
        scope.Description = "New Test Scope 1 Description";
        scope.Descriptions = "New Test Scope 1 Descriptions";
        scope.DisplayNames = "New Test Scope 1 Display Names";

        await _scopeStore.UpdateAsync(scope, CancellationToken.None);

        scope = await _scopeStore.FindByIdAsync(_testData.Scope1Id.ToString(), CancellationToken.None);
        scope.Name.ShouldBe("New Test Scope 1 Name");
        scope.DisplayName.ShouldBe("New Test Scope 1 Display Name");
        scope.Resources.ShouldBe("New Test Scope 1 Resource");
        scope.Properties.ShouldBe("New Test Scope 1 Properties");
        scope.Description.ShouldBe("New Test Scope 1 Description");
        scope.Descriptions.ShouldBe("New Test Scope 1 Descriptions");
        scope.DisplayNames.ShouldBe("New Test Scope 1 Display Names");
    }
}