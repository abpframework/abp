using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement.Blazor.Components.FeatureSettingGroup;

public partial class FeatureSettingManagementComponent : AbpComponentBase
{
    [Inject]
    protected IStringLocalizer<AbpFeatureManagementResource> L { get; set; }
    
    [Inject]
    protected PermissionChecker PermissionChecker { get; set; }
    
    protected FeatureManagementModal FeatureManagementModal;
    
    protected FeatureSettingViewModel Settings;

    protected async override Task OnInitializedAsync()
    {
        Settings = new FeatureSettingViewModel 
        {
            HasManageHostFeaturesPermission = await PermissionChecker.IsGrantedAsync(FeatureManagementPermissions.ManageHostFeatures)
        };
    }

    protected virtual async Task OnManageHostFeaturesClicked()
    {
       await FeatureManagementModal.OpenAsync(TenantFeatureValueProvider.ProviderName);
    }
}

public class FeatureSettingViewModel
{
    public bool HasManageHostFeaturesPermission { get; set; }
}