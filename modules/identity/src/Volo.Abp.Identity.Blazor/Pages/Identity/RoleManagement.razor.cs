using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Identity.Localization;
using Volo.Abp.PermissionManagement.Blazor.Components;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Data;

namespace Volo.Abp.Identity.Blazor.Pages.Identity;

public partial class RoleManagement
{
    protected const string PermissionProviderName = "R";

    protected PermissionManagementModal PermissionManagementModal;

    protected string ManagePermissionsPolicyName;

    protected bool HasManagePermissionsPermission { get; set; }

    protected PageToolbar Toolbar { get; } = new();

    protected List<TableColumn> RoleManagementTableColumns => TableColumns.Get<RoleManagement>();

    public RoleManagement()
    {
        ObjectMapperContext = typeof(AbpIdentityBlazorModule);
        LocalizationResource = typeof(IdentityResource);

        CreatePolicyName = IdentityPermissions.Roles.Create;
        UpdatePolicyName = IdentityPermissions.Roles.Update;
        DeletePolicyName = IdentityPermissions.Roles.Delete;
        ManagePermissionsPolicyName = IdentityPermissions.Roles.ManagePermissions;
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<RoleManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => { await OpenEditModalAsync(data.As<IdentityRoleDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Permissions"],
                        Visible = (data) => HasManagePermissionsPermission,
                        Clicked = async (data) =>
                        {
                            await PermissionManagementModal.OpenAsync(PermissionProviderName,
                                data.As<IdentityRoleDto>().Name);
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<IdentityRoleDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<IdentityRoleDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        RoleManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<RoleManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["RoleName"],
                        Data = nameof(IdentityRoleDto.Name),
                        Component = typeof(RoleNameComponent)
                    },
            });

        RoleManagementTableColumns.AddRange(GetExtensionTableColumns(IdentityModuleExtensionConsts.ModuleName,
            IdentityModuleExtensionConsts.EntityNames.Role));

        return base.SetTableColumnsAsync();
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManagePermissionsPermission =
            await AuthorizationService.IsGrantedAsync(IdentityPermissions.Roles.ManagePermissions);
    }

    protected override string GetDeleteConfirmationMessage(IdentityRoleDto entity)
    {
        return string.Format(L["RoleDeletionConfirmationMessage"], entity.Name);
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewRole"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }
}
