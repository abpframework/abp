using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.PermissionManagement.Application.Tests.Volo.Abp.PermissionManagement;

public class PermissionAppService_Tests : AbpPermissionManagementApplicationTestBase
{
    private readonly IPermissionAppService _permissionAppService;
    private readonly IPermissionGrantRepository _permissionGrantRepository;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
    private readonly FakePermissionChecker _fakePermissionChecker;

    public PermissionAppService_Tests()
    {
        _permissionAppService = GetRequiredService<IPermissionAppService>();
        _permissionGrantRepository = GetRequiredService<IPermissionGrantRepository>();
        _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
        _fakePermissionChecker = GetRequiredService<FakePermissionChecker>();
    }

    [Fact]
    public async Task GetAsync()
    {
        var permissionListResultDto = await _permissionAppService.GetAsync(UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());

        permissionListResultDto.ShouldNotBeNull();
        permissionListResultDto.EntityDisplayName.ShouldBe(PermissionTestDataBuilder.User1Id.ToString());

        permissionListResultDto.Groups.Count.ShouldBe(3);
        permissionListResultDto.Groups.ShouldContain(x => x.Name == "TestGroup");

        var testGroup = permissionListResultDto.Groups.FirstOrDefault(g => g.Name == "TestGroup");
        testGroup.ShouldNotBeNull();
        testGroup.Permissions.ShouldContain(x => x.Name == "MyPermission1");
        testGroup.Permissions.ShouldContain(x => x.Name == "MyPermission2");
        testGroup.Permissions.ShouldContain(x => x.Name == "MyPermission2.ChildPermission1");
        testGroup.Permissions.ShouldContain(x => x.Name == "MyPermission3");
        testGroup.Permissions.ShouldContain(x => x.Name == "MyPermission4");

        testGroup.Permissions.ShouldNotContain(x => x.Name == "MyPermission5");
        testGroup.Permissions.ShouldNotContain(x => x.Name == "MyPermission5.ChildPermission1");

        using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.Role, "super-admin")))
        {
            var result = await _permissionAppService.GetAsync(UserPermissionValueProvider.ProviderName, PermissionTestDataBuilder.User1Id.ToString());
            var testGroupWithRole = result.Groups.FirstOrDefault(g => g.Name == "TestGroup");
            testGroupWithRole.ShouldNotBeNull();
            testGroupWithRole.Permissions.ShouldContain(x => x.Name == "MyPermission5");
            testGroupWithRole.Permissions.ShouldContain(x => x.Name == "MyPermission5.ChildPermission1");
        }

        testGroup.Permissions.ShouldContain(x => x.Name == "MyPermission6");
        testGroup.Permissions.ShouldNotContain(x => x.Name == "MyPermission6.ChildDisabledPermission1");
        testGroup.Permissions.ShouldContain(x => x.Name == "MyPermission6.ChildPermission2");

        testGroup.Permissions.ShouldNotContain(x => x.Name == "MyDisabledPermission1");
        testGroup.Permissions.ShouldNotContain(x => x.Name == "MyDisabledPermission2");
        testGroup.Permissions.ShouldNotContain(x => x.Name == "MyDisabledPermission2.ChildPermission1");
        testGroup.Permissions.ShouldNotContain(x => x.Name == "MyDisabledPermission2.ChildPermission2");
        testGroup.Permissions.ShouldNotContain(x => x.Name == "MyDisabledPermission2.ChildPermission2.ChildPermission1");
    }

    [Fact]
    public async Task GetByGroupAsync()
    {
        var permissionListResultDto = await _permissionAppService.GetByGroupAsync("TestGroup2", UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());

        permissionListResultDto.ShouldNotBeNull();
        permissionListResultDto.EntityDisplayName.ShouldBe(PermissionTestDataBuilder.User1Id.ToString());
        permissionListResultDto.Groups.Count.ShouldBe(1);
        permissionListResultDto.Groups.ShouldContain(x => x.Name == "TestGroup2");

        permissionListResultDto.Groups.First().Permissions.ShouldContain(x => x.Name == "MyPermission7");
        permissionListResultDto.Groups.First().Permissions.ShouldContain(x => x.Name == "MyPermission8");
    }

    [Fact]
    public async Task UpdateAsync()
    {
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
            "Test")).ShouldBeNull();

        await _permissionAppService.UpdateAsync("Test",
            "Test", new UpdatePermissionsDto()
            {
                Permissions = new UpdatePermissionDto[]
                {
                        new UpdatePermissionDto()
                        {
                            IsGranted = true,
                            Name = "MyPermission1"
                        }
                }
            });

        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
            "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task Update_Revoke_Test()
    {
        await _permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                Guid.NewGuid(),
                "MyPermission1",
                "Test",
                "Test"
            )
        );
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
            "Test")).ShouldNotBeNull();

        await _permissionAppService.UpdateAsync("Test",
            "Test", new UpdatePermissionsDto()
            {
                Permissions = new UpdatePermissionDto[]
                {
                        new UpdatePermissionDto()
                        {
                            IsGranted = false,
                            Name = "MyPermission1"
                        }
                }
            });

        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test",
            "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task Get_Should_Mark_Permissions_As_Non_Editable_When_Current_User_Does_Not_Have_Them()
    {
        // Current user only has MyPermission1 and MyPermission2
        _fakePermissionChecker.SetGrantedPermissions("MyPermission1", "MyPermission2");

        var result = await _permissionAppService.GetAsync(
            UserPermissionValueProvider.ProviderName,
            PermissionTestDataBuilder.User1Id.ToString());

        var testGroup = result.Groups.FirstOrDefault(g => g.Name == "TestGroup");
        testGroup.ShouldNotBeNull();

        // Permissions the current user has -> IsEditable = true
        testGroup.Permissions.First(p => p.Name == "MyPermission1").IsEditable.ShouldBeTrue();
        testGroup.Permissions.First(p => p.Name == "MyPermission2").IsEditable.ShouldBeTrue();

        // Permissions the current user does NOT have -> IsEditable = false
        testGroup.Permissions.First(p => p.Name == "MyPermission2.ChildPermission1").IsEditable.ShouldBeFalse();
        testGroup.Permissions.First(p => p.Name == "MyPermission3").IsEditable.ShouldBeFalse();
        testGroup.Permissions.First(p => p.Name == "MyPermission4").IsEditable.ShouldBeFalse();
        testGroup.Permissions.First(p => p.Name == "MyPermission6").IsEditable.ShouldBeFalse();
        testGroup.Permissions.First(p => p.Name == "MyPermission6.ChildPermission2").IsEditable.ShouldBeFalse();
    }

    [Fact]
    public async Task Get_Should_Allow_Admin_To_Edit_All_Permissions()
    {
        // Current user does NOT have these permissions, but has admin role
        _fakePermissionChecker.SetGrantedPermissions("MyPermission1");

        using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.Role, "admin")))
        {
            var result = await _permissionAppService.GetAsync(
                UserPermissionValueProvider.ProviderName,
                PermissionTestDataBuilder.User1Id.ToString());

            var testGroup = result.Groups.FirstOrDefault(g => g.Name == "TestGroup");
            testGroup.ShouldNotBeNull();

            testGroup.Permissions.First(p => p.Name == "MyPermission3").IsEditable.ShouldBeTrue();
            testGroup.Permissions.First(p => p.Name == "MyPermission6").IsEditable.ShouldBeTrue();
        }
    }

    [Fact]
    public async Task Update_Should_Not_Grant_Permission_That_Current_User_Does_Not_Have()
    {
        // Current user only has MyPermission1, NOT MyPermission2
        _fakePermissionChecker.SetGrantedPermissions("MyPermission1");

        // Try to grant both MyPermission1 and MyPermission2
        await _permissionAppService.UpdateAsync("Test", "Test", new UpdatePermissionsDto()
        {
            Permissions = new UpdatePermissionDto[]
            {
                new UpdatePermissionDto() { IsGranted = true, Name = "MyPermission1" },
                new UpdatePermissionDto() { IsGranted = true, Name = "MyPermission2" }
            }
        });

        // MyPermission1 should be granted (current user has it)
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldNotBeNull();

        // MyPermission2 should NOT be granted (current user doesn't have it, filtered out)
        (await _permissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldBeNull();
    }

    [Fact]
    public async Task Update_Should_Not_Revoke_Permission_That_Current_User_Does_Not_Have()
    {
        // First, grant both permissions
        await _permissionGrantRepository.InsertAsync(
            new PermissionGrant(Guid.NewGuid(), "MyPermission1", "Test", "Test"));
        await _permissionGrantRepository.InsertAsync(
            new PermissionGrant(Guid.NewGuid(), "MyPermission2", "Test", "Test"));

        // Current user only has MyPermission1, NOT MyPermission2
        _fakePermissionChecker.SetGrantedPermissions("MyPermission1");

        // Try to revoke both
        await _permissionAppService.UpdateAsync("Test", "Test", new UpdatePermissionsDto()
        {
            Permissions = new UpdatePermissionDto[]
            {
                new UpdatePermissionDto() { IsGranted = false, Name = "MyPermission1" },
                new UpdatePermissionDto() { IsGranted = false, Name = "MyPermission2" }
            }
        });

        // MyPermission1 should be revoked (current user has it)
        (await _permissionGrantRepository.FindAsync("MyPermission1", "Test", "Test")).ShouldBeNull();

        // MyPermission2 should still be granted (current user doesn't have it, revoke filtered out)
        (await _permissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldNotBeNull();
    }

    [Fact]
    public async Task Update_Should_Allow_Admin_To_Grant_Permissions_Without_Having_Them()
    {
        (await _permissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldBeNull();

        _fakePermissionChecker.SetGrantedPermissions();

        using (_currentPrincipalAccessor.Change(new Claim(AbpClaimTypes.Role, "admin")))
        {
            await _permissionAppService.UpdateAsync("Test", "Test", new UpdatePermissionsDto()
            {
                Permissions = new UpdatePermissionDto[]
                {
                    new UpdatePermissionDto() { IsGranted = true, Name = "MyPermission2" }
                }
            });
        }

        (await _permissionGrantRepository.FindAsync("MyPermission2", "Test", "Test")).ShouldNotBeNull();
    }
}
