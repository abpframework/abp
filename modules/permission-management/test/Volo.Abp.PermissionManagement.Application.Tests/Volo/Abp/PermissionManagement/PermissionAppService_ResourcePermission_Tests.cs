using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class PermissionAppService_ResourcePermission_Tests : AbpPermissionManagementApplicationTestBase
{
    private static readonly string ResourceKey = TestEntityResource.ResourceKey7;

    private readonly IPermissionAppService _permissionAppService;
    private readonly IResourcePermissionGrantRepository _resourcePermissionGrantRepository;
    private readonly FakePermissionChecker _fakePermissionChecker;

    public PermissionAppService_ResourcePermission_Tests()
    {
        _permissionAppService = GetRequiredService<IPermissionAppService>();
        _resourcePermissionGrantRepository = GetRequiredService<IResourcePermissionGrantRepository>();
        _fakePermissionChecker = GetRequiredService<FakePermissionChecker>();
    }

    [Fact]
    public async Task GetResourceDefinitions_Should_Return_Allowed_Providers()
    {
        var result = await _permissionAppService.GetResourceDefinitionsAsync(TestEntityResource.ResourceName);

        result.Permissions.First(x => x.Name == "MyResourcePermission1").AllowedProviders.ShouldBeEmpty();
        result.Permissions.First(x => x.Name == "MyResourcePermission4").AllowedProviders.ShouldBe(new[] { "U" });
        result.Permissions.First(x => x.Name == "MyResourcePermission9").AllowedProviders.ShouldBe(new[] { "Test" });
    }

    [Fact]
    public async Task GetResourceDefinitions_Should_Not_Return_Permissions_The_Current_User_Cannot_Manage()
    {
        _fakePermissionChecker.SetGrantedPermissions();

        var result = await _permissionAppService.GetResourceDefinitionsAsync(TestEntityResource.ResourceName);

        result.Permissions.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetResourceByProvider_Should_Not_Return_Permissions_Not_Allowed_For_The_Provider()
    {
        await InsertGrantAsync("MyResourcePermission4", "Test");

        var result = await GetResourceByTestProviderAsync();

        result.Permissions.ShouldNotContain(x => x.Name == "MyResourcePermission4");
    }

    [Fact]
    public async Task GetResourceByProvider_Should_Return_Permissions_Allowed_For_The_Provider()
    {
        var result = await GetResourceByTestProviderAsync();

        result.Permissions.ShouldContain(x => x.Name == "MyResourcePermission1");
        result.Permissions.ShouldContain(x => x.Name == "MyResourcePermission9");
    }

    [Fact]
    public async Task GetResourceByProvider_Should_Return_Grant_State()
    {
        await InsertGrantAsync("MyResourcePermission1", "Test");

        var result = await GetResourceByTestProviderAsync();

        result.Permissions.ShouldContain(x => x.Name == "MyResourcePermission1" && x.IsGranted && x.Providers.Contains("Test"));
        result.Permissions.ShouldContain(x => x.Name == "MyResourcePermission2" && !x.IsGranted && x.Providers.Count == 0);
    }

    [Fact]
    public async Task UpdateResource_Should_Grant_Permissions_When_Another_Permission_Is_Not_Allowed_For_The_Provider()
    {
        await UpdateResourceForTestProviderAsync("MyResourcePermission1");

        (await FindGrantAsync("MyResourcePermission1", "Test")).ShouldNotBeNull();
        (await FindGrantAsync("MyResourcePermission4", "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task UpdateResource_Should_Ignore_Requested_Permissions_Not_Allowed_For_The_Provider()
    {
        await UpdateResourceForTestProviderAsync("MyResourcePermission1", "MyResourcePermission4");

        (await FindGrantAsync("MyResourcePermission1", "Test")).ShouldNotBeNull();
        (await FindGrantAsync("MyResourcePermission4", "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task UpdateResource_Should_Grant_Permissions_Restricted_To_The_Provider()
    {
        await UpdateResourceForTestProviderAsync("MyResourcePermission9");

        (await FindGrantAsync("MyResourcePermission9", "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task UpdateResource_Should_Revoke_Permissions_Not_In_The_Input()
    {
        await UpdateResourceForTestProviderAsync("MyResourcePermission1", "MyResourcePermission2");
        await UpdateResourceForTestProviderAsync("MyResourcePermission1");

        (await FindGrantAsync("MyResourcePermission1", "Test")).ShouldNotBeNull();
        (await FindGrantAsync("MyResourcePermission2", "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task UpdateResource_Should_Revoke_All_Permissions_When_Input_Is_Empty()
    {
        await UpdateResourceForTestProviderAsync("MyResourcePermission1", "MyResourcePermission9");
        await UpdateResourceForTestProviderAsync();

        (await FindGrantAsync("MyResourcePermission1", "Test")).ShouldBeNull();
        (await FindGrantAsync("MyResourcePermission9", "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task UpdateResource_Should_Not_Change_Permissions_The_Current_User_Cannot_Manage()
    {
        await InsertGrantAsync("MyResourcePermission2", "Test");
        _fakePermissionChecker.SetGrantedPermissions();

        await UpdateResourceForTestProviderAsync("MyResourcePermission1");

        (await FindGrantAsync("MyResourcePermission1", "Test")).ShouldBeNull();
        (await FindGrantAsync("MyResourcePermission2", "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task UpdateResource_Should_Keep_Existing_Grants_Not_Allowed_For_The_Provider()
    {
        await InsertGrantAsync("MyResourcePermission4", "Test");

        await UpdateResourceForTestProviderAsync();

        (await FindGrantAsync("MyResourcePermission4", "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task DeleteResource_Should_Delete_All_Grants_Of_The_Provider()
    {
        await InsertGrantAsync("MyResourcePermission1", "Test");
        await InsertGrantAsync("MyResourcePermission4", "Test");
        await InsertGrantAsync("MyResourcePermission1", "Test", "AnotherKey");

        await _permissionAppService.DeleteResourceAsync(TestEntityResource.ResourceName, ResourceKey, "Test", "Test");

        (await FindGrantAsync("MyResourcePermission1", "Test")).ShouldBeNull();
        (await FindGrantAsync("MyResourcePermission4", "Test")).ShouldBeNull();
        (await FindGrantAsync("MyResourcePermission1", "Test", "AnotherKey")).ShouldNotBeNull();
    }

    [Fact]
    public async Task GetResource_Should_Group_Grants_By_Provider()
    {
        await InsertGrantAsync("MyResourcePermission1", "Test");
        await InsertGrantAsync("MyResourcePermission2", "Test");

        var result = await _permissionAppService.GetResourceAsync(TestEntityResource.ResourceName, ResourceKey);

        var grant = result.Permissions.ShouldHaveSingleItem();
        grant.ProviderName.ShouldBe("Test");
        grant.ProviderKey.ShouldBe("Test");
        grant.Permissions.Select(x => x.Name).ShouldBe(new[] { "MyResourcePermission1", "MyResourcePermission2" }, ignoreOrder: true);
    }

    private Task<GetResourcePermissionWithProviderListResultDto> GetResourceByTestProviderAsync()
    {
        return _permissionAppService.GetResourceByProviderAsync(TestEntityResource.ResourceName, ResourceKey, "Test", "Test");
    }

    private Task UpdateResourceForTestProviderAsync(params string[] permissions)
    {
        return _permissionAppService.UpdateResourceAsync(
            TestEntityResource.ResourceName,
            ResourceKey,
            new UpdateResourcePermissionsDto
            {
                ProviderName = "Test",
                ProviderKey = "Test",
                Permissions = new List<string>(permissions)
            });
    }

    private Task<ResourcePermissionGrant> InsertGrantAsync(string permissionName, string providerName, string providerKey = "Test")
    {
        return _resourcePermissionGrantRepository.InsertAsync(new ResourcePermissionGrant(
            Guid.NewGuid(),
            permissionName,
            TestEntityResource.ResourceName,
            ResourceKey,
            providerName,
            providerKey));
    }

    private Task<ResourcePermissionGrant> FindGrantAsync(string permissionName, string providerName, string providerKey = "Test")
    {
        return _resourcePermissionGrantRepository.FindAsync(permissionName, TestEntityResource.ResourceName, ResourceKey, providerName, providerKey);
    }
}
