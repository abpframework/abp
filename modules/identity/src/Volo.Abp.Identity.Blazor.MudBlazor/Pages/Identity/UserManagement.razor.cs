using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.PageToolbars;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Localization;
using Volo.Abp.MudBlazorUI;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement.Blazor.MudBlazor.Components;
using Volo.Abp.Users;

namespace Volo.Abp.Identity.Blazor.MudBlazor.Pages.Identity;

public partial class UserManagement
{
    [Parameter]
    public string? Culture { get; set; }

    protected const string PermissionProviderName = "U";

    protected const string DefaultSelectedTab = "UserInformations";

    protected PermissionManagementModal? PermissionManagementModal;

    protected IReadOnlyList<IdentityRoleDto>? Roles;

    protected AssignedRoleViewModel[]? NewUserRoles;

    protected AssignedRoleViewModel[]? EditUserRoles;

    protected string? ManagePermissionsPolicyName;

    protected bool HasManagePermissionsPermission { get; set; }

    protected int _createTabIndex = 0;
    protected int _editTabIndex = 0;
    protected bool _showPassword;
    protected bool IsEditCurrentUser;

    protected PageToolbar Toolbar { get; } = new();

    private List<TableColumn> UserManagementTableColumns => TableColumns.Get<UserManagement>();
    private AbpMudBlazorMessageLocalizerHelper<IdentityResource>? _localizerHelper;

    [Inject]
    protected IPermissionChecker PermissionChecker { get; set; } = default!;

    [Inject]
    protected IStringLocalizer<IdentityResource> Localizer { get; set; } = default!;

    public UserManagement()
    {
        ObjectMapperContext = typeof(AbpIdentityBlazorMudBlazorModule);
        LocalizationResource = typeof(IdentityResource);

        CreatePolicyName = IdentityPermissions.Users.Create;
        UpdatePolicyName = IdentityPermissions.Users.Update;
        DeletePolicyName = IdentityPermissions.Users.Delete;
        ManagePermissionsPolicyName = IdentityPermissions.Users.ManagePermissions;
    }

    protected override async Task OnInitializedAsync()
    {
        _localizerHelper = new AbpMudBlazorMessageLocalizerHelper<IdentityResource>(Localizer);
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

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(LUiNavigation["Menu:Administration"].Value,  null, disabled: true));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:IdentityManagement"].Value,  null, disabled: true));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Users"].Value,  null, disabled: true));
        return base.SetBreadcrumbItemsAsync();
    }

    protected virtual async Task OnSearchKeyUp(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            CurrentPage = 1;
            await _dataGrid.ReloadServerDataAsync();
        }
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManagePermissionsPermission =
            await AuthorizationService.IsGrantedAsync(IdentityPermissions.Users.ManagePermissions);
    }

    protected override async Task OpenCreateDialogAsync()
    {
        _createTabIndex = 0;

        if (Roles != null)
        {
            NewUserRoles = Roles.Select(x => new AssignedRoleViewModel
            {
                Name = x.Name,
                IsAssigned = x.IsDefault,
                IsAssignable = true
            }).ToArray();
        }

        _showPassword = false;
        await base.OpenCreateDialogAsync();

        NewEntity.IsActive = true;
        NewEntity.LockoutEnabled = true;
    }

    protected override Task OnCreatingEntityAsync()
    {
        // apply roles before saving
        if (NewUserRoles != null)
        {
            NewEntity.RoleNames = NewUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();
        }

        return base.OnCreatingEntityAsync();
    }

    protected override async Task OpenEditDialogAsync(IdentityUserDto entity)
    {
        try
        {
            _editTabIndex = 0;
            IsEditCurrentUser = entity.Id == CurrentUser.Id;

            if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageRoles))
            {
                var assignableRoles = Roles ?? (await AppService.GetAssignableRolesAsync()).Items;
                var currentRoles = (await AppService.GetRolesAsync(entity.Id)).Items;

                var combinedRoles = assignableRoles
                    .Concat(currentRoles)
                    .GroupBy(role => role.Id)
                    .Select(group => group.First())
                    .ToList();

                var currentRoleIds = currentRoles.Select(r => r.Id).ToHashSet();
                var assignableRoleIds = assignableRoles.Select(r => r.Id).ToHashSet();

                EditUserRoles = combinedRoles.Select(x => new AssignedRoleViewModel
                {
                    Name = x.Name,
                    IsAssigned = currentRoleIds.Contains(x.Id),
                    IsAssignable = assignableRoleIds.Contains(x.Id)
                }).ToArray();

                _showPassword = false;
            }
            await base.OpenEditDialogAsync(entity);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected override Task OnUpdatingEntityAsync()
    {
        // apply roles before saving
        if (EditUserRoles != null)
        {
            EditingEntity.RoleNames = EditUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();
        }
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
                        Clicked = async (data) => await OpenEditDialogAsync(data.As<IdentityUserDto>())
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
                                    data.As<IdentityUserDto>().Id.ToString(),
                                    data.As<IdentityUserDto>().UserName);
                            }
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

    protected override async ValueTask SetTableColumnsAsync()
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

        UserManagementTableColumns.AddRange(await GetExtensionTableColumnsAsync(IdentityModuleExtensionConsts.ModuleName,
            IdentityModuleExtensionConsts.EntityNames.User));
        await base.SetTableColumnsAsync();
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddMudButton(L["NewUser"], OpenCreateDialogAsync,
            icon: Icons.Material.Filled.Add.ToString(),
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }
}

public class AssignedRoleViewModel
{
    public string Name { get; set; } = string.Empty;

    public bool IsAssigned { get; set; }

    public bool IsAssignable { get; set; }
}
