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
using Volo.Abp.FeatureManagement.Blazor.MudBlazor.Components;
using Volo.Abp.MudBlazorUI;
using Volo.Abp.ObjectExtending;
using Volo.Abp.TenantManagement.Localization;

namespace Volo.Abp.TenantManagement.Blazor.MudBlazor.Pages.TenantManagement;

public partial class TenantManagement
{
    [Parameter]
    public string? Culture { get; set; }

    protected const string FeatureProviderName = "T";

    protected bool HasManageFeaturesPermission;
    protected string? ManageFeaturesPolicyName;

    protected FeatureManagementModal? FeatureManagementModal;

    protected bool ShowPassword { get; set; }

    protected PageToolbar Toolbar { get; } = new();

    protected List<TableColumn> TenantManagementTableColumns => TableColumns.Get<TenantManagement>();

    private AbpMudBlazorMessageLocalizerHelper<AbpTenantManagementResource>? _localizerHelper;

    [Inject]
    protected IStringLocalizer<AbpTenantManagementResource> Localizer { get; set; } = default!;

    private MudForm? _createFormRef;
    private MudForm? _editFormRef;

    public TenantManagement()
    {
        LocalizationResource = typeof(AbpTenantManagementResource);
        ObjectMapperContext = typeof(AbpTenantManagementBlazorMudBlazorModule);

        CreatePolicyName = TenantManagementPermissions.Tenants.Create;
        UpdatePolicyName = TenantManagementPermissions.Tenants.Update;
        DeletePolicyName = TenantManagementPermissions.Tenants.Delete;

        ManageFeaturesPolicyName = TenantManagementPermissions.Tenants.ManageFeatures;
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(LUiNavigation["Menu:Administration"].Value,  null, disabled: true));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:TenantManagement"].Value,  null, disabled: true));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Tenants"].Value,  null, disabled: true));
        return base.SetBreadcrumbItemsAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        _localizerHelper = new AbpMudBlazorMessageLocalizerHelper<AbpTenantManagementResource>(Localizer);
        await base.OnInitializedAsync();
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManageFeaturesPermission = await AuthorizationService.IsGrantedAsync(ManageFeaturesPolicyName!);
    }

    protected override string GetDeleteConfirmationMessage(TenantDto entity)
    {
        return string.Format(L["TenantDeletionConfirmationMessage"], entity.Name);
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddMudButton(L["NewTenant"],
            OpenCreateDialogAsync,
            icon: Icons.Material.Filled.Add.ToString(),
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<TenantManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => { await OpenEditDialogAsync(data.As<TenantDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Features"],
                        Visible = (data) => HasManageFeaturesPermission,
                        Clicked = async (data) =>
                        {
                            var tenant = data.As<TenantDto>();
                            if (FeatureManagementModal != null)
                            {
                                await FeatureManagementModal.OpenAsync(FeatureProviderName, tenant.Id.ToString(), tenant.Name);
                            }
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<TenantDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<TenantDto>())
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override async ValueTask SetTableColumnsAsync()
    {
        TenantManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<TenantManagement>(),
                    },
                    new TableColumn
                    {
                        Title = L["TenantName"],
                        Sortable = true,
                        Data = nameof(TenantDto.Name),
                    },
            });

        TenantManagementTableColumns.AddRange(await GetExtensionTableColumnsAsync(
            TenantManagementModuleExtensionConsts.ModuleName,
            TenantManagementModuleExtensionConsts.EntityNames.Tenant));

        await base.SetTableColumnsAsync();
    }

    protected virtual void TogglePasswordVisibility()
    {
        ShowPassword = !ShowPassword;
    }

    protected override async Task CreateEntityAsync()
    {
        if (_createFormRef == null)
        {
            return;
        }

        await _createFormRef.ValidateAsync();
        if (!_createFormRef.IsValid)
        {
            return;
        }

        await base.CreateEntityAsync();
    }

    protected override async Task UpdateEntityAsync()
    {
        if (_editFormRef == null)
        {
            return;
        }

        await _editFormRef.ValidateAsync();
        if (!_editFormRef.IsValid)
        {
            return;
        }
        
        await base.UpdateEntityAsync();
    }
}
