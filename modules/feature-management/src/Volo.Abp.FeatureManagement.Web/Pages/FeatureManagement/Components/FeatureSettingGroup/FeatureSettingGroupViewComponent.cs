using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement.Pages.FeatureManagement.Components.FeatureSettingGroup;

public class FeatureSettingGroupViewComponent: AbpViewComponent
{
    protected IPermissionChecker PermissionChecker { get; }
    
    public FeatureSettingGroupViewComponent(IPermissionChecker permissionChecker)
    {
        PermissionChecker = permissionChecker;
    }
    
    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new FeatureSettingViewModel
        {
            HasManageHostFeaturesPermission = await PermissionChecker.IsGrantedAsync(FeatureManagementPermissions.ManageHostFeatures)
        };
        return View("~/Pages/FeatureManagement/Components/FeatureSettingGroup/Default.cshtml", model);
    }
    
    public class FeatureSettingViewModel
    {
        public bool HasManageHostFeaturesPermission { get; set; }
    }
}