using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityDynamicClaimsPrincipalContributor_Tests : AbpIdentityDomainTestBase
{
    private readonly IdentityUserManager _identityUserManager;
    private readonly IIdentityRoleRepository _identityRoleRepository;
    private readonly IdentityRoleManager _identityRoleManager;
    private readonly IOrganizationUnitRepository _organizationUnitRepository;
    private readonly OrganizationUnitManager _organizationUnitManager;
    private readonly IAbpClaimsPrincipalFactory _abpClaimsPrincipalFactory;
    private readonly AbpUserClaimsPrincipalFactory _abpUserClaimsPrincipalFactory;
    private readonly IdentityTestData _testData;

    public IdentityDynamicClaimsPrincipalContributor_Tests()
    {
        _identityUserManager = GetRequiredService<IdentityUserManager>();
        _identityRoleRepository = GetRequiredService<IIdentityRoleRepository>();
        _identityRoleManager = GetRequiredService<IdentityRoleManager>();
        _organizationUnitRepository = GetRequiredService<IOrganizationUnitRepository>();
        _organizationUnitManager = GetRequiredService<OrganizationUnitManager>();
        _abpClaimsPrincipalFactory = GetRequiredService<IAbpClaimsPrincipalFactory>();
        _abpUserClaimsPrincipalFactory = GetRequiredService<AbpUserClaimsPrincipalFactory>();
        _testData = GetRequiredService<IdentityTestData>();
    }

    [Fact]
    public async Task Should_Get_Correct_Claims_After_User_Updated()
    {
        IdentityUser user = null;
        ClaimsPrincipal claimsPrincipal = null;
        string securityStamp = null;
        await UsingUowAsync(async () =>
        {
            user = await _identityUserManager.GetByIdAsync(_testData.UserJohnId);
            user.ShouldNotBeNull();
            securityStamp = user.SecurityStamp;
            claimsPrincipal = await _abpUserClaimsPrincipalFactory.CreateAsync(user);

            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.NameIdentifier && x.Value == user.Id.ToString());
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Name && x.Value == user.UserName);
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == user.Email);
            claimsPrincipal.Claims.ShouldContain(x => x.Type == "AspNet.Identity.SecurityStamp" && x.Value == securityStamp);
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");

            var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.NameIdentifier && x.Value == user.Id.ToString());
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Name && x.Value == user.UserName);
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == user.Email);
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == "AspNet.Identity.SecurityStamp" && x.Value == securityStamp);//SecurityStamp is not dynamic claim
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");

            await _identityUserManager.SetUserNameAsync(user, "newUserName");
            await _identityUserManager.SetEmailAsync(user, "newUserEmail@abp.io");
            await _identityUserManager.UpdateSecurityStampAsync(user);
        });

        var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.NameIdentifier && x.Value == user.Id.ToString());
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Name && x.Value =="newUserName");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Email && x.Value == "newUserEmail@abp.io");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == "AspNet.Identity.SecurityStamp" && x.Value == securityStamp);//SecurityStamp is not dynamic claim
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");
    }

    [Fact]
    public async Task Should_Get_Correct_Claims_After_User_Role_Updated()
    {
        ClaimsPrincipal claimsPrincipal = null;
        await UsingUowAsync(async () =>
        {
            var user = await _identityUserManager.GetByIdAsync(_testData.UserJohnId);
            user.ShouldNotBeNull();
            claimsPrincipal = await _abpUserClaimsPrincipalFactory.CreateAsync(user);

            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");

            var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");

            var roles = (await _identityRoleRepository.GetListAsync()).Where(x => user.Roles.Select(r => r.RoleId).Contains(x.Id)).ToList();

            var role = roles.First(x => x.Name == "supporter");
            await _identityRoleManager.SetRoleNameAsync(role, "newSupporter");
            await _identityRoleRepository.UpdateAsync(role);
        });

        var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "newSupporter");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");
    }

    [Fact]
    public async Task Should_Get_Correct_Claims_After_User_Role_Deleted()
    {
        ClaimsPrincipal claimsPrincipal = null;
        await UsingUowAsync(async () =>
        {
            var user = await _identityUserManager.GetByIdAsync(_testData.UserJohnId);
            user.ShouldNotBeNull();
            claimsPrincipal = await _abpUserClaimsPrincipalFactory.CreateAsync(user);

            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");

            var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");

            var roles = (await _identityRoleRepository.GetListAsync()).Where(x => user.Roles.Select(r => r.RoleId).Contains(x.Id)).ToList();

            await _identityRoleManager.DeleteAsync(roles.First(x => x.Name == "supporter"));
            await _identityRoleManager.DeleteAsync(roles.First(x => x.Name == "moderator"));
        });

        var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
        dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
        dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");
    }

    [Fact]
    public async Task Should_Get_Correct_Claims_After_User_Organization_Updated()
    {
        ClaimsPrincipal claimsPrincipal = null;
        await UsingUowAsync(async () =>
        {
            var user = await _identityUserManager.GetByIdAsync(_testData.UserJohnId);
            user.ShouldNotBeNull();
            claimsPrincipal = await _abpUserClaimsPrincipalFactory.CreateAsync(user);

            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");

            var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");

            var ou = await _organizationUnitRepository.GetAsync("OU111", true);
            ou.ShouldNotBeNull();
            ou.Roles.Count.ShouldBe(2);
            ou.Roles.ShouldContain(x => x.RoleId == _testData.RoleModeratorId);
            ou.Roles.ShouldContain(x => x.RoleId == _testData.RoleManagerId);

            ou.AddRole(_testData.RoleSaleId);
            await _organizationUnitManager.UpdateAsync(ou);
        });

        var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "sale");
    }

    [Fact]
    public async Task Should_Get_Correct_Claims_After_User_Organization_Deleted()
    {
        ClaimsPrincipal claimsPrincipal = null;
        await UsingUowAsync(async () =>
        {
            var user = await _identityUserManager.GetByIdAsync(_testData.UserJohnId);
            user.ShouldNotBeNull();
            claimsPrincipal = await _abpUserClaimsPrincipalFactory.CreateAsync(user);

            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");

            var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");

            var ou = await _organizationUnitRepository.GetAsync("OU111", true);
            ou.ShouldNotBeNull();
            ou.Roles.Count.ShouldBe(2);
            ou.Roles.ShouldContain(x => x.RoleId == _testData.RoleModeratorId);
            ou.Roles.ShouldContain(x => x.RoleId == _testData.RoleManagerId);
            var users = await _organizationUnitRepository.GetMemberIdsAsync(ou.Id);
            users.ShouldContain(user.Id);

            await _organizationUnitManager.DeleteAsync(ou.Id);
        });

        var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
        dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "manager"); //manager role from OU111 is deleted.
    }

    [Fact]
    public async Task Should_Get_Correct_Claims_After_User_Organization_Role_Or_Member_Changed()
    {
        ClaimsPrincipal claimsPrincipal = null;
        await UsingUowAsync(async () =>
        {
            var user = await _identityUserManager.GetByIdAsync(_testData.UserBobId);
            user.ShouldNotBeNull();
            claimsPrincipal = await _abpUserClaimsPrincipalFactory.CreateAsync(user);
            claimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            claimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            claimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");
            var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
            dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
            dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
            dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");
            var ou = await _organizationUnitRepository.GetAsync("OU111", true);
            ou.ShouldNotBeNull();
            ou.Roles.Count.ShouldBe(2);
            ou.Roles.ShouldContain(x => x.RoleId == _testData.RoleModeratorId);
            ou.Roles.ShouldContain(x => x.RoleId == _testData.RoleManagerId);
            ou.AddRole(_testData.RoleSaleId);
            await _identityUserManager.AddToOrganizationUnitAsync(user, ou);
            await _organizationUnitManager.UpdateAsync(ou);
        });
        var dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
        dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "sale");

        await UsingUowAsync(async () =>
        {
            var saleRole = await _identityRoleRepository.GetAsync(_testData.RoleSaleId);
            await _identityRoleManager.DeleteAsync(saleRole);
        });

        dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
        dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");
        dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "sale");

        await UsingUowAsync(async () =>
        {
            var user = await _identityUserManager.GetByIdAsync(_testData.UserBobId);
            user.ShouldNotBeNull();
            var ou = await _organizationUnitRepository.GetAsync("OU111", true);
            ou.ShouldNotBeNull();
            await _identityUserManager.RemoveFromOrganizationUnitAsync(user.Id, ou.Id);
        });

        dynamicClaimsPrincipal = await _abpClaimsPrincipalFactory.CreateDynamicAsync(claimsPrincipal);
        dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "supporter");
        dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "moderator");
        dynamicClaimsPrincipal.Claims.ShouldContain(x => x.Type == ClaimTypes.Role && x.Value == "manager");
        dynamicClaimsPrincipal.Claims.ShouldNotContain(x => x.Type == ClaimTypes.Role && x.Value == "sale");
    }
}
