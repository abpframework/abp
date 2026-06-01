using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.Identity;

public class IdentityUserAppService_Tests : AbpIdentityApplicationTestBase
{
    private readonly IIdentityUserAppService _userAppService;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IPermissionManager _permissionManager;
    private readonly UserPermissionManagementProvider _userPermissionManagementProvider;
    private readonly IdentityTestData _testData;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public IdentityUserAppService_Tests()
    {
        _userAppService = GetRequiredService<IIdentityUserAppService>();
        _userRepository = GetRequiredService<IIdentityUserRepository>();
        _permissionManager = GetRequiredService<IPermissionManager>();
        _userPermissionManagementProvider = GetRequiredService<UserPermissionManagementProvider>();
        _testData = GetRequiredService<IdentityTestData>();
        _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
    }

    [Fact]
    public async Task GetAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        //Act

        var result = await _userAppService.GetAsync(johnNash.Id);

        //Assert

        result.Id.ShouldBe(johnNash.Id);
        result.UserName.ShouldBe(johnNash.UserName);
        result.Email.ShouldBe(johnNash.Email);
        result.LockoutEnabled.ShouldBe(johnNash.LockoutEnabled);
        result.PhoneNumber.ShouldBe(johnNash.PhoneNumber);
    }

    [Fact]
    public async Task GetListAsync()
    {
        //Act

        var result = await _userAppService.GetListAsync(new GetIdentityUsersInput());

        //Assert

        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task CreateAsync()
    {
        //Arrange

        var input = new IdentityUserCreateDto
        {
            UserName = Guid.NewGuid().ToString(),
            Email = CreateRandomEmail(),
            LockoutEnabled = true,
            PhoneNumber = CreateRandomPhoneNumber(),
            Password = "123qwE4r*",
            RoleNames = new[] { "moderator" }
        };

        //Act

        var result = await _userAppService.CreateAsync(input);

        //Assert

        result.Id.ShouldNotBe(Guid.Empty);
        result.UserName.ShouldBe(input.UserName);
        result.Email.ShouldBe(input.Email);
        result.LockoutEnabled.ShouldBe(input.LockoutEnabled);
        result.PhoneNumber.ShouldBe(input.PhoneNumber);

        var user = await _userRepository.GetAsync(result.Id);
        user.Id.ShouldBe(result.Id);
        user.UserName.ShouldBe(input.UserName);
        user.Email.ShouldBe(input.Email);
        user.LockoutEnabled.ShouldBe(input.LockoutEnabled);
        user.PhoneNumber.ShouldBe(input.PhoneNumber);
    }

    [Fact]
    public async Task UpdateAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        var input = new IdentityUserUpdateDto
        {
            UserName = johnNash.UserName,
            LockoutEnabled = true,
            PhoneNumber = CreateRandomPhoneNumber(),
            Password = "123qwe4R*",
            Email = CreateRandomEmail(),
            RoleNames = new[] { "admin", "moderator" },
            ConcurrencyStamp = johnNash.ConcurrencyStamp,
            Surname = johnNash.Surname,
            Name = johnNash.Name
        };

        //Act

        var result = await _userAppService.UpdateAsync(johnNash.Id, input);

        //Assert

        result.Id.ShouldBe(johnNash.Id);
        result.UserName.ShouldBe(input.UserName);
        result.Email.ShouldBe(input.Email);
        result.LockoutEnabled.ShouldBe(input.LockoutEnabled);
        result.PhoneNumber.ShouldBe(input.PhoneNumber);

        var user = await _userRepository.GetAsync(result.Id);
        user.Id.ShouldBe(result.Id);
        user.UserName.ShouldBe(input.UserName);
        user.Email.ShouldBe(input.Email);
        user.LockoutEnabled.ShouldBe(input.LockoutEnabled);
        user.PhoneNumber.ShouldBe(input.PhoneNumber);
        user.Roles.Count.ShouldBe(2);
    }


    [Fact]
    public async Task UpdateAsync_Concurrency_Exception()
    {
        //Get user
        var johnNash = await _userAppService.GetAsync(_testData.UserJohnId);

        //Act

        var input = new IdentityUserUpdateDto
        {
            Name = "John-updated",
            Surname = "Nash-updated",
            UserName = johnNash.UserName,
            LockoutEnabled = true,
            PhoneNumber = CreateRandomPhoneNumber(),
            Email = CreateRandomEmail(),
            RoleNames = new[] { "admin", "moderator" },
            ConcurrencyStamp = johnNash.ConcurrencyStamp
        };

        await _userAppService.UpdateAsync(johnNash.Id, input);

        //Second update with same input will throw exception because the entity has been modified
        (await Assert.ThrowsAsync<AbpIdentityResultException>(async () =>
        {
            await _userAppService.UpdateAsync(johnNash.Id, input);
        })).Message.ShouldContain("Optimistic concurrency check has been failed. The entity you're working on has modified by another user. Please discard your changes and try again.");
    }

    [Fact]
    public async Task DeleteAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        //Act

        await _userAppService.DeleteAsync(johnNash.Id);

        //Assert

        FindUser("john.nash").ShouldBeNull();
    }

    [Fact]
    public async Task User_Permissions_Should_Deleted_If_User_Deleted()
    {
        var johnNash = GetUser("john.nash");

        (await _userPermissionManagementProvider.CheckAsync(IdentityPermissions.Users.Create, UserPermissionValueProvider.ProviderName, johnNash.Id.ToString())).IsGranted.ShouldBeFalse();
        await _permissionManager.SetForUserAsync(johnNash.Id, IdentityPermissions.Users.Create, true);
        (await _userPermissionManagementProvider.CheckAsync(IdentityPermissions.Users.Create, UserPermissionValueProvider.ProviderName, johnNash.Id.ToString())).IsGranted.ShouldBeTrue();

        await _userAppService.DeleteAsync(johnNash.Id);
        (await _userPermissionManagementProvider.CheckAsync(IdentityPermissions.Users.Create, UserPermissionValueProvider.ProviderName, johnNash.Id.ToString())).IsGranted.ShouldBeFalse();
    }

    [Fact]
    public async Task GetRolesAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        //Act

        var result = await _userAppService.GetRolesAsync(johnNash.Id);

        //Assert

        result.Items.Count.ShouldBe(3);
        result.Items.ShouldContain(r => r.Name == "moderator");
        result.Items.ShouldContain(r => r.Name == "supporter");
        result.Items.ShouldContain(r => r.Name == "manager");
    }

    [Fact]
    public async Task UpdateRolesAsync()
    {
        //Arrange

        var johnNash = GetUser("john.nash");

        //Act

        await _userAppService.UpdateRolesAsync(
            johnNash.Id,
            new IdentityUserUpdateRolesDto
            {
                RoleNames = new[] { "admin", "moderator" }
            }
        );

        //Assert

        var roleNames = await _userRepository.GetRoleNamesAsync(johnNash.Id);
        roleNames.Count.ShouldBe(3);
        roleNames.ShouldContain("admin");
        roleNames.ShouldContain("moderator");
        roleNames.ShouldContain("manager");
    }

    [Fact]
    public async Task UpdateRolesAsync_Should_Not_Assign_Roles_Operator_Does_Not_Have()
    {
        // neo only has "supporter" role
        using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.UserId, _testData.UserNeoId.ToString())))
        {
            // Try to assign "admin" and "supporter" to david (who has no roles)
            await _userAppService.UpdateRolesAsync(
                _testData.UserDavidId,
                new IdentityUserUpdateRolesDto
                {
                    RoleNames = new[] { "admin", "supporter" }
                }
            );
        }

        // Only "supporter" should be assigned (admin filtered out since neo doesn't have it)
        var roleNames = await _userRepository.GetRoleNamesAsync(_testData.UserDavidId);
        roleNames.ShouldContain("supporter");
        roleNames.ShouldNotContain("admin");
    }

    [Fact]
    public async Task UpdateRolesAsync_Should_Preserve_Unmanageable_Roles()
    {
        // john.nash has direct roles: moderator, supporter
        // neo only has "supporter" role, so "moderator" is unmanageable for neo
        using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.UserId, _testData.UserNeoId.ToString())))
        {
            await _userAppService.UpdateRolesAsync(
                _testData.UserJohnId,
                new IdentityUserUpdateRolesDto
                {
                    RoleNames = new[] { "supporter" }
                }
            );
        }

        // "moderator" should be preserved (unmanageable), "supporter" kept (in input)
        var roleNames = await _userRepository.GetRoleNamesAsync(_testData.UserJohnId);
        roleNames.ShouldContain("moderator");
        roleNames.ShouldContain("supporter");
    }

    [Fact]
    public async Task UpdateRolesAsync_Should_Only_Remove_Manageable_Roles()
    {
        // john.nash has direct roles: moderator, supporter
        // neo only has "supporter" role
        using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.UserId, _testData.UserNeoId.ToString())))
        {
            // Input is empty - try to remove all roles
            await _userAppService.UpdateRolesAsync(
                _testData.UserJohnId,
                new IdentityUserUpdateRolesDto
                {
                    RoleNames = Array.Empty<string>()
                }
            );
        }

        // "moderator" should be preserved (neo can't manage it), "supporter" removed (neo has it and it's not in input)
        var roleNames = await _userRepository.GetRoleNamesAsync(_testData.UserJohnId);
        roleNames.ShouldContain("moderator");
        roleNames.ShouldNotContain("supporter");
    }

    [Fact]
    public async Task UpdateRolesAsync_Admin_Can_Assign_Any_Role()
    {
        // admin user can assign roles they do not have (e.g. "sale")
        using (_currentPrincipalAccessor.Change(new[]
               {
                   new Claim(AbpClaimTypes.UserId, _testData.UserAdminId.ToString()),
                   new Claim(AbpClaimTypes.Role, "admin")
               }))
        {
            await _userAppService.UpdateRolesAsync(
                _testData.UserDavidId,
                new IdentityUserUpdateRolesDto
                {
                    RoleNames = new[] { "sale" }
                }
            );
        }

        var roleNames = await _userRepository.GetRoleNamesAsync(_testData.UserDavidId);
        roleNames.ShouldContain("sale");
    }

    [Fact]
    public async Task UpdateRolesAsync_Self_Cannot_Add_New_Roles()
    {
        // neo only has "supporter", tries to add "admin" to self
        using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.UserId, _testData.UserNeoId.ToString())))
        {
            await _userAppService.UpdateRolesAsync(
                _testData.UserNeoId,
                new IdentityUserUpdateRolesDto
                {
                    RoleNames = new[] { "supporter", "admin" }
                }
            );
        }

        // "admin" should not be added (neo doesn't have it), "supporter" kept
        var roleNames = await _userRepository.GetRoleNamesAsync(_testData.UserNeoId);
        roleNames.ShouldContain("supporter");
        roleNames.ShouldNotContain("admin");
    }

    [Fact]
    public async Task UpdateRolesAsync_Self_Can_Remove_Own_Roles()
    {
        // admin user has: admin, moderator, supporter, manager
        // Remove supporter and manager from self
        await _userAppService.UpdateRolesAsync(
            _testData.UserAdminId,
            new IdentityUserUpdateRolesDto
            {
                RoleNames = new[] { "admin", "moderator" }
            }
        );

        var roleNames = await _userRepository.GetRoleNamesAsync(_testData.UserAdminId);
        roleNames.ShouldContain("admin");
        roleNames.ShouldContain("moderator");
        roleNames.ShouldNotContain("supporter");
        roleNames.ShouldNotContain("manager");
    }

    private static string CreateRandomEmail()
    {
        return Guid.NewGuid().ToString("N").Left(16) + "@abp.io";
    }

    private static string CreateRandomPhoneNumber()
    {
        return RandomHelper.GetRandom(10000000, 100000000).ToString();
    }
}
