using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public abstract class UserManagementBase : AbpCrudPageBase<IIdentityUserAppService, IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>
    {
        protected const string PermissionProviderName = "U";

        protected const string DefaultSelectedTab = "UserInformations";

        protected PermissionManagementModal PermissionManagementModal;

        protected IReadOnlyList<IdentityRoleDto> Roles;

        protected AssignedRoleViewModel[] NewUserRoles;

        protected AssignedRoleViewModel[] EditUserRoles;

        protected bool ShouldShowEntityActions { get; set; }
        protected bool HasManagePermissionsPermission { get; set; }

        protected string CreateModalSelectedTab = DefaultSelectedTab;

        protected string EditModalSelectedTab = DefaultSelectedTab;

        public UserManagementBase()
        {
            ObjectMapperContext = typeof(AbpIdentityBlazorModule);

            CreatePolicyName = IdentityPermissions.Users.Create;
            UpdatePolicyName = IdentityPermissions.Users.Update;
            DeletePolicyName = IdentityPermissions.Users.Delete;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Roles = (await AppService.GetAssignableRolesAsync()).Items;
        }

        protected override async Task SetPermissionsAsync()
        {
            await base.SetPermissionsAsync();

            HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(
                IdentityPermissions.Users.ManagePermissions
            );

            ShouldShowEntityActions = HasUpdatePermission ||
                                      HasDeletePermission ||
                                      HasManagePermissionsPermission;
        }

        protected override Task OpenCreateModalAsync()
        {
            CreateModalSelectedTab = DefaultSelectedTab;

            NewUserRoles = Roles.Select(x => new AssignedRoleViewModel
                            {
                                Name = x.Name,
                                IsAssigned = x.IsDefault
                            }).ToArray();

            return base.OpenCreateModalAsync();
        }

        protected override Task CreateEntityAsync()
        {
            NewEntity.RoleNames = NewUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();

            return base.CreateEntityAsync();
        }

        protected override async Task OpenEditModalAsync(Guid id)
        {
            EditModalSelectedTab = DefaultSelectedTab;

            var userRoleNames = (await AppService.GetRolesAsync(id)).Items.Select(r => r.Name).ToList();

            EditUserRoles = Roles.Select(x => new AssignedRoleViewModel
                                {
                                    Name = x.Name,
                                    IsAssigned = userRoleNames.Contains(x.Name)
                                }).ToArray();

            await base.OpenEditModalAsync(id);
        }

        protected override Task UpdateEntityAsync()
        {
            EditingEntity.RoleNames = EditUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();

            return base.UpdateEntityAsync();
        }
    }

    public class AssignedRoleViewModel
    {
        public string Name { get; set; }

        public bool IsAssigned { get; set; }
    }
}
