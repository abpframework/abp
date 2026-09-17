using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Authorization.TestServices.Resources;
using Xunit;

namespace Volo.Abp.Authorization;

public class ResourcePermissionChecker_BulkWithStateChecker_Tests : AuthorizationTestBase
{
    private readonly IResourcePermissionChecker _resourcePermissionChecker;

    public ResourcePermissionChecker_BulkWithStateChecker_Tests()
    {
        _resourcePermissionChecker = GetRequiredService<IResourcePermissionChecker>();
    }

    [Fact]
    public async Task IsGrantedAsync_With_Empty_Names_Should_Return_Empty_Result()
    {
        var result = await _resourcePermissionChecker.IsGrantedAsync(
            Array.Empty<string>(), TestEntityResource.ResourceName, TestEntityResource.ResourceKey5);

        result.Result.ShouldBeEmpty();
    }

    [Fact]
    public async Task IsGrantedAsync_StateChecker_Permission_Is_Undefined_When_StateChecker_Fails()
    {
        // MyResourcePermission1 has TestRequireEditionPermissionSimpleStateChecker.
        // Current user (Douglas) has no EditionId claim → StateChecker returns false.
        // The permission must stay Undefined (never reaches the value-provider pipeline).
        var result = await _resourcePermissionChecker.IsGrantedAsync(
            new[] { "MyResourcePermission1", "MyResourcePermission3" },
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey3);

        result.Result["MyResourcePermission1"].ShouldBe(PermissionGrantResult.Undefined);
        result.Result["MyResourcePermission3"].ShouldBe(PermissionGrantResult.Granted);
    }

    [Fact]
    public async Task IsGrantedAsync_Mix_Of_Defined_And_Undefined_Permissions()
    {
        var result = await _resourcePermissionChecker.IsGrantedAsync(
            new[] { "MyResourcePermission3", "NonExistentPermission", "MyResourcePermission5" },
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey5);

        result.Result["MyResourcePermission3"].ShouldBe(PermissionGrantResult.Granted);
        result.Result["NonExistentPermission"].ShouldBe(PermissionGrantResult.Prohibited);
        result.Result["MyResourcePermission5"].ShouldBe(PermissionGrantResult.Granted);
    }

}
