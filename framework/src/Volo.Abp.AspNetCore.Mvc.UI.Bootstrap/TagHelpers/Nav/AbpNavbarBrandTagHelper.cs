using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    [HtmlTargetElement(Attributes = "abp-navbar-brand")]
    public class AbpNavbarBrandTagHelper : AbpTagHelper<AbpNavbarBrandTagHelper, AbpNavbarBrandTagHelperService>
    {

        public AbpNavbarBrandTagHelper(AbpNavbarBrandTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
