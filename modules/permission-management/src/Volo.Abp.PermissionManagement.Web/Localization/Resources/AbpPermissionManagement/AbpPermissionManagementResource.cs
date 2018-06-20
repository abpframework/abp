using Localization.Resources.AbpUi;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;

namespace Volo.Abp.PermissionManagement.Web.Localization.Resources.AbpPermissionManagement
{
    [InheritResource(
        typeof(AbpValidationResource), 
        typeof(AbpUiResource))]
    [LocalizationResourceName("AbpPermissionManagement")]
    public class AbpPermissionManagementResource
    {
        
    }
}
