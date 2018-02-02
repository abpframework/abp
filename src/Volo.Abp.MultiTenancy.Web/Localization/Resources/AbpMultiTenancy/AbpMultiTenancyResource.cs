using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Localization.Resources.AbpBootstrap;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;

namespace Volo.Abp.MultiTenancy.Web.Localization.Resources.AbpMultiTenancy
{
    [InheritResource(
        typeof(AbpValidationResource),
        typeof(AbpBootstrapResource))]
    [ShortLocalizationResourceName("AbpMultiTenancy")]
    public class AbpMultiTenancyResource
    {
        
    }
}
