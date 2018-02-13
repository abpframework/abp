using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Localization.Resources.AbpBootstrap;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;

namespace Volo.Abp.Permissions.Web.Localization.Resources.AbpPermissions
{
    [InheritResource(
        typeof(AbpValidationResource), 
        typeof(AbpBootstrapResource))]
    [ShortLocalizationResourceName("AbpPermissions")]
    public class AbpPermissionsResource
    {
        
    }
}
