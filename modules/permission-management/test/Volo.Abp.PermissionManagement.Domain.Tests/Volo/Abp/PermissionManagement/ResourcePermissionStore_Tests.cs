using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionStore_Tests : PermissionTestBase
{
    private readonly IResourcePermissionStore _resourcePermissionStore;

    public ResourcePermissionStore_Tests()
    {
        _resourcePermissionStore = GetRequiredService<IResourcePermissionStore>();
    }

    [Fact]
    public async Task IsGrantedAsync()
    {
        (await _resourcePermissionStore.IsGrantedAsync(
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString())).ShouldBeTrue();

        (await _resourcePermissionStore.IsGrantedAsync(
            "MyPermission1NotExist",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString())).ShouldBeFalse();
    }

    [Fact]
    public async Task IsGranted_Multiple()
    {
        var result = await _resourcePermissionStore.IsGrantedAsync(
            new[] { "MyResourcePermission1", "MyResourcePermission1NotExist" },
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1,
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());

        result.Result.Count.ShouldBe(2);

        result.Result.FirstOrDefault(x => x.Key == "MyResourcePermission1").Value.ShouldBe(PermissionGrantResult.Granted);
        result.Result.FirstOrDefault(x => x.Key == "MyResourcePermission1NotExist").Value.ShouldBe(PermissionGrantResult.Undefined);
    }


    [Fact]
    public async Task GetPermissionsAsync()
    {
        var permissions = await _resourcePermissionStore.GetPermissionsAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1);

        permissions.Result.Count.ShouldBe(10);
        permissions.Result.ShouldContain(p => p.Key == "MyResourcePermission1" && p.Value == PermissionGrantResult.Granted);
        permissions.Result.ShouldContain(p => p.Key == "MyResourceDisabledPermission1" && p.Value == PermissionGrantResult.Undefined);
        permissions.Result.ShouldContain(p => p.Key == "MyResourcePermission2" && p.Value == PermissionGrantResult.Undefined);
        permissions.Result.ShouldContain(p => p.Key == "MyResourcePermission3" && p.Value == PermissionGrantResult.Undefined);
        permissions.Result.ShouldContain(p => p.Key == "MyResourcePermission4" && p.Value == PermissionGrantResult.Undefined);
        permissions.Result.ShouldContain(p => p.Key == "MyResourcePermission5" && p.Value == PermissionGrantResult.Undefined);
        permissions.Result.ShouldContain(p => p.Key == "MyResourcePermission6" && p.Value == PermissionGrantResult.Undefined);
        permissions.Result.ShouldContain(p => p.Key == "MyResourceDisabledPermission2" && p.Value == PermissionGrantResult.Undefined);
        permissions.Result.ShouldContain(p => p.Key == "MyResourcePermission7" && p.Value == PermissionGrantResult.Undefined);
        permissions.Result.ShouldContain(p => p.Key == "MyResourcePermission8" && p.Value == PermissionGrantResult.Undefined);

        permissions = await _resourcePermissionStore.GetPermissionsAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey2);
        permissions.Result.ShouldAllBe(x => x.Value == PermissionGrantResult.Undefined);
    }

    [Fact]
    public async Task GetGrantedPermissionsAsync()
    {
        var grantedPermissions = await _resourcePermissionStore.GetGrantedPermissionsAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1);

        grantedPermissions.Length.ShouldBe(1);
        grantedPermissions.ShouldContain("MyResourcePermission1");

        grantedPermissions = await _resourcePermissionStore.GetGrantedPermissionsAsync(
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey3);

        grantedPermissions.Length.ShouldBe(3);
        grantedPermissions.ShouldContain("MyResourcePermission3");
        grantedPermissions.ShouldContain("MyResourcePermission5");
        grantedPermissions.ShouldContain("MyResourcePermission6");

    }


    [Fact]
    public async Task GetGrantedResourceKeysAsync()
    {
        var grantedResourceKeys = await _resourcePermissionStore.GetGrantedResourceKeysAsync(
            TestEntityResource.ResourceName,
            "MyResourcePermission1");

        grantedResourceKeys.Length.ShouldBe(1);
        grantedResourceKeys.ShouldContain(TestEntityResource.ResourceKey1);

        grantedResourceKeys = await _resourcePermissionStore.GetGrantedResourceKeysAsync(
            TestEntityResource.ResourceName,
            "MyResourcePermission5");

        grantedResourceKeys.Length.ShouldBe(2);
        grantedResourceKeys.ShouldContain(TestEntityResource.ResourceKey3);
        grantedResourceKeys.ShouldContain(TestEntityResource.ResourceKey5);
    }
}
