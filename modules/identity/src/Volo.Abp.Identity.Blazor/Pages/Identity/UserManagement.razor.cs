using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Extensibility;
using Volo.Abp.AspNetCore.Components.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Extensibility.TableColumns;
using Volo.Abp.Identity.Localization;
using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public partial class UserManagement
    {
        protected const string PermissionProviderName = "U";

        protected const string DefaultSelectedTab = "UserInformations";

        protected PermissionManagementModal PermissionManagementModal;

        protected IReadOnlyList<IdentityRoleDto> Roles;

        protected AssignedRoleViewModel[] NewUserRoles;

        protected AssignedRoleViewModel[] EditUserRoles;

        protected string ManagePermissionsPolicyName;
        
        protected bool HasManagePermissionsPermission { get; set; }

        protected string CreateModalSelectedTab = DefaultSelectedTab;

        protected string EditModalSelectedTab = DefaultSelectedTab;

        public UserManagement()
        {
            ObjectMapperContext = typeof(AbpIdentityBlazorModule);
            LocalizationResource = typeof(IdentityResource);

            CreatePolicyName = IdentityPermissions.Users.Create;
            UpdatePolicyName = IdentityPermissions.Users.Update;
            DeletePolicyName = IdentityPermissions.Users.Delete;
            ManagePermissionsPolicyName = IdentityPermissions.Users.ManagePermissions;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Roles = (await AppService.GetAssignableRolesAsync()).Items;
        }
        
        protected override async Task SetPermissionsAsync()
        {
            await base.SetPermissionsAsync();

            HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(IdentityPermissions.Users.ManagePermissions);
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

        protected override Task OnCreatingEntityAsync()
        {
            // apply roles before saving
            NewEntity.RoleNames = NewUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();

            return base.OnCreatingEntityAsync();
        }

        protected override async Task OpenEditModalAsync(IdentityUserDto entity)
        {
            EditModalSelectedTab = DefaultSelectedTab;

            var userRoleNames = (await AppService.GetRolesAsync(entity.Id)).Items.Select(r => r.Name).ToList();

            EditUserRoles = Roles.Select(x => new AssignedRoleViewModel
            {
                Name = x.Name,
                IsAssigned = userRoleNames.Contains(x.Name)
            }).ToArray();

            await base.OpenEditModalAsync(entity);
        }

        protected override Task OnUpdatingEntityAsync()
        {
            // apply roles before saving
            EditingEntity.RoleNames = EditUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();

            return base.OnUpdatingEntityAsync();
        }

        protected override string GetDeleteConfirmationMessage(IdentityUserDto entity)
        {
            return string.Format(L["UserDeletionConfirmationMessage"], entity.UserName);
        }

        protected override ValueTask SetEntityActionsAsync()
        {
            UIExtensions.Instance
                .EntityActions
                .Get<UserManagement>()
                .AddIfNotContains(new EntityAction[]
                {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        RequiredPolicy = UpdatePolicyName,
                        Clicked = async (data) => await OpenEditModalAsync(data.As<IdentityUserDto>())
                    },
                    new EntityAction
                    {
                        Text = L["Permissions"],
                        RequiredPolicy = ManagePermissionsPolicyName,
                        Clicked = async (data) =>
                        {
                            await PermissionManagementModal.OpenAsync(PermissionProviderName,
                                data.As<IdentityUserDto>().Id.ToString());
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        RequiredPolicy = DeletePolicyName,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<IdentityUserDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<IdentityUserDto>())
                    }
                });

            return ValueTask.CompletedTask;
        }

        protected override ValueTask SetTableColumnsAsync()
        {
            UIExtensions.Instance
                .TableColumns
                .Get<UserManagement>()
                .AddIfNotContains(new TableColumn[]
                {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = UIExtensions.Instance.EntityActions.Get<UserManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["UserName"],
                        Data = nameof(IdentityUserDto.UserName),
                    },
                    new TableColumn
                    {
                        Title = L["Email"],
                        Data = nameof(IdentityUserDto.Email),
                    },
                    new TableColumn
                    {
                        Title = L["PhoneNumber"],
                        Data = nameof(IdentityUserDto.PhoneNumber),
                    }
                });
            
            return ValueTask.CompletedTask;
        }
    }

    public class AssignedRoleViewModel
    {
        public string Name { get; set; }

        public bool IsAssigned { get; set; }
    }
}
