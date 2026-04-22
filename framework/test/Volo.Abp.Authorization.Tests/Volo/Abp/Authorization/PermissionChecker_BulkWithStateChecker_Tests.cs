using System;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Xunit;

namespace Volo.Abp.Authorization;

public class PermissionChecker_BulkWithStateChecker_Tests : AuthorizationTestBase
{
    private readonly IPermissionChecker _permissionChecker;

    public PermissionChecker_BulkWithStateChecker_Tests()
    {
        _permissionChecker = GetRequiredService<IPermissionChecker>();
    }

    [Fact]
    public async Task IsGrantedAsync_With_Empty_Names_Should_Return_Empty_Result()
    {
        var result = await _permissionChecker.IsGrantedAsync(Array.Empty<string>());
        result.Result.ShouldBeEmpty();
    }

    [Fact]
    public async Task IsGrantedAsync_StateChecker_Permission_Is_Undefined_When_StateChecker_Fails()
    {
        // MyPermission1 has TestRequireEditionPermissionSimpleStateChecker.
        // Current user (Douglas) has no EditionId claim → StateChecker returns false.
        // The permission must stay Undefined (never reaches the value-provider pipeline).
        var result = await _permissionChecker.IsGrantedAsync(new[] { "MyPermission1", "MyPermission3" });

        result.Result["MyPermission1"].ShouldBe(PermissionGrantResult.Undefined);
        result.Result["MyPermission3"].ShouldBe(PermissionGrantResult.Granted);
    }

    [Fact]
    public async Task IsGrantedAsync_Mix_Of_Defined_And_Undefined_Permissions()
    {
        var result = await _permissionChecker.IsGrantedAsync(new[]
        {
            "MyPermission3",
            "NonExistentPermission",
            "MyPermission5"
        });

        result.Result["MyPermission3"].ShouldBe(PermissionGrantResult.Granted);
        result.Result["NonExistentPermission"].ShouldBe(PermissionGrantResult.Prohibited);
        result.Result["MyPermission5"].ShouldBe(PermissionGrantResult.Granted);
    }
}
