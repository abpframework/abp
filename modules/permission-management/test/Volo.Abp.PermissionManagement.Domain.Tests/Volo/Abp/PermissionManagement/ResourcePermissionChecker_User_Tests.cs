using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionChecker_User_Tests : PermissionTestBase
{
    private readonly IResourcePermissionChecker _resourcePermissionChecker;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public ResourcePermissionChecker_User_Tests()
    {
        _resourcePermissionChecker = GetRequiredService<IResourcePermissionChecker>();
        _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
    }

    [Fact]
    public async Task Should_Return_True_For_Granted_Current_User()
    {
        (await _resourcePermissionChecker.IsGrantedAsync(
            CreatePrincipal(PermissionTestDataBuilder.User1Id),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1
        )).ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Return_False_For_Non_Granted_Current_User()
    {
        (await _resourcePermissionChecker.IsGrantedAsync(
            CreatePrincipal(PermissionTestDataBuilder.User2Id),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1
        )).ShouldBeFalse();
    }


    [Fact]
    public async Task Should_Return_False_For_Granted_Current_User_If_The_Permission_Is_Disabled()
    {
        //Disabled permissions always returns false!
        (await _resourcePermissionChecker.IsGrantedAsync(
            CreatePrincipal(PermissionTestDataBuilder.User1Id),
            "MyDisabledPermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1
        )).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Return_False_For_Current_User_If_Anonymous()
    {
        (await _resourcePermissionChecker.IsGrantedAsync(
            CreatePrincipal(null),
            "MyResourcePermission1",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey1
        )).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Not_Allow_Host_Permission_To_Tenant_User_Even_Granted_Before()
    {
        (await _resourcePermissionChecker.IsGrantedAsync(
            CreatePrincipal(PermissionTestDataBuilder.User1Id, Guid.NewGuid()),
            "MyResourcePermission3",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey3
        )).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Return_False_For_Granted_Current_User_If_The_Permission_State_Is_Disabled()
    {
        (await _resourcePermissionChecker.IsGrantedAsync(
            CreatePrincipal(PermissionTestDataBuilder.User1Id, Guid.NewGuid()),
            "MyResourcePermission5",
            TestEntityResource.ResourceName,
            TestEntityResource.ResourceKey5
        )).ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Return_True_For_Granted_Current_User_If_The_Permission_State_Is_Enabled()
    {
        using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.Role, "super-admin")))
        {
            (await _resourcePermissionChecker.IsGrantedAsync(
                CreatePrincipal(PermissionTestDataBuilder.User1Id, Guid.NewGuid()),
                "MyResourcePermission5",
                TestEntityResource.ResourceName,
                TestEntityResource.ResourceKey5
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
