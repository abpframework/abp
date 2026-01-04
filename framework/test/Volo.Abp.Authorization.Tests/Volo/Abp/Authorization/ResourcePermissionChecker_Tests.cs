using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Authorization.TestServices.Resources;
using Xunit;

namespace Volo.Abp.Authorization;

public class ResourcePermissionChecker_Tests: AuthorizationTestBase
{
    private readonly IResourcePermissionChecker _resourcePermissionChecker;

    public ResourcePermissionChecker_Tests()
    {
        _resourcePermissionChecker = GetRequiredService<IResourcePermissionChecker>();
    }

    [Fact]
    public async Task IsGrantedAsync()
    {
        (await _resourcePermissionChecker.IsGrantedAsync("MyResourcePermission5", TestEntityResource.ResourceName, TestEntityResource.ResourceKey5)).ShouldBe(true);
        (await _resourcePermissionChecker.IsGrantedAsync("UndefinedResourcePermission", TestEntityResource.ResourceName, TestEntityResource.ResourceKey5)).ShouldBe(false);
        (await _resourcePermissionChecker.IsGrantedAsync("MyResourcePermission8", TestEntityResource.ResourceName, TestEntityResource.ResourceKey5)).ShouldBe(false);
    }

    [Fact]
    public async Task IsGranted_Multiple_Result_Async()
    {
        var result = await _resourcePermissionChecker.IsGrantedAsync(new []
        {
            "MyResourcePermission1",
            "MyResourcePermission2",
            "UndefinedPermission",
            "MyResourcePermission3",
            "MyResourcePermission4",
            "MyResourcePermission5",
            "MyResourcePermission8"
        }, TestEntityResource.ResourceName, TestEntityResource.ResourceKey5);

        result.Result["MyResourcePermission1"].ShouldBe(PermissionGrantResult.Undefined);
        result.Result["MyResourcePermission2"].ShouldBe(PermissionGrantResult.Prohibited);
        result.Result["UndefinedPermission"].ShouldBe(PermissionGrantResult.Prohibited);
        result.Result["MyResourcePermission3"].ShouldBe(PermissionGrantResult.Granted);
        result.Result["MyResourcePermission4"].ShouldBe(PermissionGrantResult.Prohibited);
        result.Result["MyResourcePermission5"].ShouldBe(PermissionGrantResult.Granted);
        result.Result["MyResourcePermission8"].ShouldBe(PermissionGrantResult.Prohibited);

        result = await _resourcePermissionChecker.IsGrantedAsync(new []
        {
            "MyResourcePermission6",
        }, TestEntityResource.ResourceName, TestEntityResource.ResourceKey6);

        result.Result["MyResourcePermission6"].ShouldBe(PermissionGrantResult.Granted);

        result = await _resourcePermissionChecker.IsGrantedAsync(new []
        {
            "MyResourcePermission7",
        }, TestEntityResource.ResourceName, TestEntityResource.ResourceKey7);
        result.Result["MyResourcePermission7"].ShouldBe(PermissionGrantResult.Granted);
    }
}
