﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Identity.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement.Blazor.Components;
using Volo.Abp.Users;

namespace Volo.Abp.Identity.Blazor.Pages.Identity;

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

    protected PageToolbar Toolbar { get; } = new();

    private List<TableColumn> UserManagementTableColumns => TableColumns.Get<UserManagement>();
    private TextRole _passwordTextRole = TextRole.Password;
    
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

        try
        {
            Roles = (await AppService.GetAssignableRolesAsync()).Items;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OnSearchTextChanged(string value)
    {
        GetListInput.Filter = value;
        CurrentPage = 1;
        await GetEntitiesAsync();
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManagePermissionsPermission =
            await AuthorizationService.IsGrantedAsync(IdentityPermissions.Users.ManagePermissions);
    }

    protected override Task OpenCreateModalAsync()
    {
        CreateModalSelectedTab = DefaultSelectedTab;

        NewUserRoles = Roles.Select(x => new AssignedRoleViewModel
        {
            Name = x.Name,
            IsAssigned = x.IsDefault
        }).ToArray();

        ChangePasswordTextRole(TextRole.Password);
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
        try
        {
            EditModalSelectedTab = DefaultSelectedTab;

            var userRoleNames = (await AppService.GetRolesAsync(entity.Id)).Items.Select(r => r.Name).ToList();

            EditUserRoles = Roles.Select(x => new AssignedRoleViewModel
            {
                Name = x.Name,
                IsAssigned = userRoleNames.Contains(x.Name)
            }).ToArray();

            ChangePasswordTextRole(TextRole.Password);
            await base.OpenEditModalAsync(entity);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
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
        EntityActions
            .Get<UserManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => await OpenEditModalAsync(data.As<IdentityUserDto>())
                    },
                    new EntityAction
                    {
                        Text = L["Permissions"],
                        Visible = (data) => HasManagePermissionsPermission,
                        Clicked = async (data) =>
                        {
                            await PermissionManagementModal.OpenAsync(PermissionProviderName,
                                data.As<IdentityUserDto>().Id.ToString(),
                                data.As<IdentityUserDto>().UserName);
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission && CurrentUser.GetId() != data.As<IdentityUserDto>().Id,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<IdentityUserDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<IdentityUserDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        UserManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<UserManagement>(),
                    },
                    new TableColumn
                    {
                        Title = L["UserName"],
                        Data = nameof(IdentityUserDto.UserName),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["EmailAddress"],
                        Data = nameof(IdentityUserDto.Email),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["PhoneNumber"],
                        Data = nameof(IdentityUserDto.PhoneNumber),
                        Sortable = true,
                    }
            });

        UserManagementTableColumns.AddRange(GetExtensionTableColumns(IdentityModuleExtensionConsts.ModuleName,
            IdentityModuleExtensionConsts.EntityNames.User));
        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewUser"], OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected virtual void ChangePasswordTextRole(TextRole? textRole)
    {
        if (textRole == null)
        {
            ChangePasswordTextRole(_passwordTextRole == TextRole.Password ? TextRole.Text: TextRole.Password);
        }
        else
        {
            _passwordTextRole = textRole.Value;
        }
    }
}

public class AssignedRoleViewModel
{
    public string Name { get; set; }

    public bool IsAssigned { get; set; }
}
