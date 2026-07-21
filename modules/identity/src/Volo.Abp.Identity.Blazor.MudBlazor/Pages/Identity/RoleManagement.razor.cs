using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.PageToolbars;
using Volo.Abp.Identity.Localization;
using Volo.Abp.MudBlazorUI;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement.Blazor.MudBlazor.Components;

namespace Volo.Abp.Identity.Blazor.MudBlazor.Pages.Identity;

public partial class RoleManagement
{
    [Parameter]
    public string? Culture { get; set; }

    protected const string PermissionProviderName = "R";

    protected PermissionManagementModal? PermissionManagementModal;

    protected string? ManagePermissionsPolicyName;

    protected bool HasManagePermissionsPermission { get; set; }

    protected PageToolbar Toolbar { get; } = new();

    protected List<TableColumn> RoleManagementTableColumns => TableColumns.Get<RoleManagement>();

    private AbpMudBlazorMessageLocalizerHelper<IdentityResource>? _localizerHelper;

    [Inject]
    protected IStringLocalizer<IdentityResource> Localizer { get; set; } = default!;

    public RoleManagement()
    {
        ObjectMapperContext = typeof(AbpIdentityBlazorMudBlazorModule);
        LocalizationResource = typeof(IdentityResource);

        CreatePolicyName = IdentityPermissions.Roles.Create;
        UpdatePolicyName = IdentityPermissions.Roles.Update;
        DeletePolicyName = IdentityPermissions.Roles.Delete;
        ManagePermissionsPolicyName = IdentityPermissions.Roles.ManagePermissions;
    }

    protected override async Task OnInitializedAsync()
    {
        _localizerHelper = new AbpMudBlazorMessageLocalizerHelper<IdentityResource>(Localizer);
        await base.OnInitializedAsync();
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(LUiNavigation["Menu:Administration"].Value,  null, disabled: true));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:IdentityManagement"].Value,  null, disabled: true));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Roles"].Value,  null, disabled: true));
        return base.SetBreadcrumbItemsAsync();
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
                        Clicked = async (data) => { await OpenEditDialogAsync(data.As<IdentityRoleDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Permissions"],
                        Visible = (data) => HasManagePermissionsPermission,
                        Clicked = async (data) =>
                        {
                            if (PermissionManagementModal != null)
                            {
                                await PermissionManagementModal.OpenAsync(PermissionProviderName,
                                    data.As<IdentityRoleDto>().Name);
                            }
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission && !data.As<IdentityRoleDto>().IsStatic,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<IdentityRoleDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<IdentityRoleDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override async ValueTask SetTableColumnsAsync()
    {
        RoleManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<RoleManagement>(),
                    },
                    new TableColumn
                    {
                        Title = L["RoleName"],
                        Sortable = true,
                        Data = nameof(IdentityRoleDto.Name),
                        Component = typeof(RoleNameComponent)
                    },
            });

        RoleManagementTableColumns.AddRange(await GetExtensionTableColumnsAsync(IdentityModuleExtensionConsts.ModuleName,
            IdentityModuleExtensionConsts.EntityNames.Role));

        await base.SetTableColumnsAsync();
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
        Toolbar.AddMudButton(L["NewRole"],
            OpenCreateDialogAsync,
            icon: Icons.Material.Filled.Add.ToString(),
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }
}
