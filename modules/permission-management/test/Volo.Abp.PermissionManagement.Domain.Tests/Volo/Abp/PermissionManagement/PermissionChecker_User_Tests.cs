using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class PermissionChecker_User_Tests : PermissionTestBase
{
    private readonly IPermissionChecker _permissionChecker;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public PermissionChecker_User_Tests()
    {
        _permissionChecker = GetRequiredService<IPermissionChecker>();
        _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
    }

    [Fact]
    public async Task Should_Return_True_For_Granted_Current_User()
    {
        (await _permissionChecker.IsGrantedAsync(
            CreatePrincipal(PermissionTestDataBuilder.User1Id),
            "MyPermission1"
        )).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Return_False_For_Non_Granted_Current_User()
    {
        (await _permissionChecker.IsGrantedAsync(
            CreatePrincipal(PermissionTestDataBuilder.User2Id),
            "MyPermission1"
        )).ShouldBeFalse();
    }


    [Fact]
    public async Task Should_Return_False_For_Granted_Current_User_If_The_Permission_Is_Disabled()
    {
        //Disabled permissions always returns false!
        (await _permissionChecker.IsGrantedAsync(
            CreatePrincipal(PermissionTestDataBuilder.User1Id),
            "MyDisabledPermission1"
        )).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Return_False_For_Current_User_If_Anonymous()
    {
        (await _permissionChecker.IsGrantedAsync(
            CreatePrincipal(null),
            "MyPermission1"
        )).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Not_Allow_Host_Permission_To_Tenant_User_Even_Granted_Before()
    {
        (await _permissionChecker.IsGrantedAsync(
            CreatePrincipal(PermissionTestDataBuilder.User1Id, Guid.NewGuid()),
            "MyPermission3"
        )).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Return_False_For_Granted_Current_User_If_The_Permission_State_Is_Disabled()
    {
        (await _permissionChecker.IsGrantedAsync(
            CreatePrincipal(PermissionTestDataBuilder.User1Id, Guid.NewGuid()),
            "MyPermission5"
        )).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Return_True_For_Granted_Current_User_If_The_Permission_State_Is_Enabled()
    {
        using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.Role, "super-admin")))
        {
            (await _permissionChecker.IsGrantedAsync(
                CreatePrincipal(PermissionTestDataBuilder.User1Id, Guid.NewGuid()),
                "MyPermission5"
            )).ShouldBeTrue();
        }
    }

    private static ClaimsPrincipal CreatePrincipal(Guid? userId, Guid? tenantId = null)
    {
        var claimsIdentity = new ClaimsIdentity();

        if (userId != null)
        {
            claimsIdentity.AddClaim(new Claim(AbpClaimTypes.UserId, userId.ToString()));
        }

        if (tenantId != null)
        {
            claimsIdentity.AddClaim(new Claim(AbpClaimTypes.TenantId, tenantId.ToString()));
        }

        return new ClaimsPrincipal(claimsIdentity);
    }
}
