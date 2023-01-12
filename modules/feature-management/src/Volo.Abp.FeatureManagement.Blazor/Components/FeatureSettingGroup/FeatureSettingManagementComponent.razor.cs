using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement.Blazor.Components.FeatureSettingGroup;

public partial class FeatureSettingManagementComponent : AbpComponentBase
{
    [Inject]
    protected PermissionChecker PermissionChecker { get; set; }
    
    protected FeatureManagementModal FeatureManagementModal;
    
    protected FeatureSettingViewModel Settings;

    public FeatureSettingManagementComponent()
    {
        LocalizationResource = typeof(AbpFeatureManagementResource);
    }

    protected async override Task OnInitializedAsync()
    {
        Settings = new FeatureSettingViewModel 
        {
            HasManageHostFeaturesPermission = await AuthorizationService.IsGrantedAsync(FeatureManagementPermissions.ManageHostFeatures)
        };
    }

    protected virtual async Task OnManageHostFeaturesClicked()
    {
       await FeatureManagementModal.OpenAsync(TenantFeatureValueProvider.ProviderName);
    }
}