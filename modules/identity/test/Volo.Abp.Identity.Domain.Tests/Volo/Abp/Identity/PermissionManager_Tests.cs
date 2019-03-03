using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.PermissionManagement;
using Xunit;

namespace Volo.Abp.Identity
{
    public class PermissionManager_Tests : AbpIdentityDomainTestBase
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IPermissionStore _permissionStore;

        public PermissionManager_Tests()
        {
            _permissionManager = GetRequiredService<IPermissionManager>();
            _permissionStore = GetRequiredService<IPermissionStore>();
        }

        [Fact]
        public async Task Roles_Should_Have_Configured_Permissions()
        {
            //admin
            var grantInfos = await _permissionManager.GetAllForRoleAsync("admin");
            RoleShouldHavePermission(grantInfos, "admin", TestPermissionNames.MyPermission1);
            RoleShouldHavePermission(grantInfos, "admin", TestPermissionNames.MyPermission2);
            RoleShouldHavePermission(grantInfos, "admin", TestPermissionNames.MyPermission2_ChildPermission1);

            //moderator
            grantInfos = await _permissionManager.GetAllForRoleAsync("moderator");
            RoleShouldHavePermission(grantInfos, "moderator", TestPermissionNames.MyPermission1);
            RoleShouldHavePermission(grantInfos, "moderator", TestPermissionNames.MyPermission2);
            ShouldNotHavePermission(grantInfos, TestPermissionNames.MyPermission2_ChildPermission1);

            //supporter
            grantInfos = await _permissionManager.GetAllForRoleAsync("supporter");
            RoleShouldHavePermission(grantInfos, "supporter", TestPermissionNames.MyPermission1);
            ShouldNotHavePermission(grantInfos, TestPermissionNames.MyPermission2);
            ShouldNotHavePermission(grantInfos, TestPermissionNames.MyPermission2_ChildPermission1);
        }

        [Fact]
        public async Task Should_Grant_Permission_To_Role()
        {
            (await _permissionManager.GetForRoleAsync("supporter", TestPermissionNames.MyPermission2)).IsGranted.ShouldBeFalse();
            (await _permissionStore.IsGrantedAsync(TestPermissionNames.MyPermission2, RolePermissionValueProvider.ProviderName, "supporter")).ShouldBeFalse();

            await _permissionManager.SetForRoleAsync("supporter", TestPermissionNames.MyPermission2, true);

            (await _permissionManager.GetForRoleAsync("supporter", TestPermissionNames.MyPermission2)).IsGranted.ShouldBeTrue();
            (await _permissionStore.IsGrantedAsync(TestPermissionNames.MyPermission2, RolePermissionValueProvider.ProviderName, "supporter")).ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Revoke_Permission_From_Role()
        {
            (await _permissionManager.GetForRoleAsync("moderator", TestPermissionNames.MyPermission1)).IsGranted.ShouldBeTrue();
            await _permissionManager.SetForRoleAsync("moderator", TestPermissionNames.MyPermission1, false);
            (await _permissionManager.GetForRoleAsync("moderator", TestPermissionNames.MyPermission1)).IsGranted.ShouldBeFalse();
        }

        [Fact]
        public async Task Users_Should_Have_Configured_Values()
        {
            //administrator
            var user = GetUser("administrator");
            var grantInfos = await _permissionManager.GetAllForUserAsync(user.Id);
            UserShouldHavePermission(grantInfos, user.Id, TestPermissionNames.MyPermission1, "admin");
            UserShouldHavePermission(grantInfos, user.Id, TestPermissionNames.MyPermission2, "admin");
            UserShouldHavePermission(grantInfos, user.Id, TestPermissionNames.MyPermission2_ChildPermission1, "admin");

            //john.nash
            user = GetUser("john.nash");
            grantInfos = await _permissionManager.GetAllForUserAsync(user.Id);
            UserShouldHavePermission(grantInfos, user.Id, TestPermissionNames.MyPermission1, "moderator", "supporter");
            UserShouldHavePermission(grantInfos, user.Id, TestPermissionNames.MyPermission2, "moderator");
            ShouldNotHavePermission(grantInfos, TestPermissionNames.MyPermission2_ChildPermission1);

            //john.nash
            user = GetUser("david");
            grantInfos = await _permissionManager.GetAllForUserAsync(user.Id);
            UserShouldHavePermission(grantInfos, user.Id, TestPermissionNames.MyPermission1);
            ShouldNotHavePermission(grantInfos, TestPermissionNames.MyPermission2);
            ShouldNotHavePermission(grantInfos, TestPermissionNames.MyPermission2_ChildPermission1);
        }

        private static void RoleShouldHavePermission(List<PermissionWithGrantedProviders> grantInfos, string roleName, string permissionName)
        {
            grantInfos.ShouldContain(
                p => p.Name == permissionName &&
                     p.IsGranted &&
                     p.Providers.Count == 1 &&
                     p.Providers.Any(
                         pr => pr.Name == RolePermissionValueProvider.ProviderName &&
                               pr.Key == roleName
                     )
            );
        }

        private static void UserShouldHavePermission(List<PermissionWithGrantedProviders> grantInfos, Guid userId, string permissionName, params string[] inheritedRolesForThisPermission)
        {
            grantInfos.ShouldContain(
                p => p.Name == permissionName &&
                     p.IsGranted
            );
        }

        private static void ShouldNotHavePermission(List<PermissionWithGrantedProviders> grantInfos, string permissionName)
        {
            grantInfos.ShouldContain(
                p => p.Name == permissionName &&
                     !p.IsGranted &&
                     p.Providers.Count == 0
            );
        }
    }
}
