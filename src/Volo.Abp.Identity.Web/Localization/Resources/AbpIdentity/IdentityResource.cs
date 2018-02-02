using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Localization.Resources.AbpBootstrap;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;

namespace Volo.Abp.Identity.Web.Localization.Resources.AbpIdentity
{
    [InheritResource(
        typeof(AbpValidationResource), 
        typeof(AbpBootstrapResource))]
    [ShortLocalizationResourceName("AbpIdentity")]
    public class IdentityResource //TODO: Rename to AbpIdentityResource
    {
        
    }
}
