using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    [HtmlTargetElement("span", Attributes = "abp-navbar-text")]
    public class AbpNavbarTextTagHelper : AbpTagHelper<AbpNavbarTextTagHelper, AbpNavbarTextTagHelperService>
    {
        public AbpNavbarTextTagHelper(AbpNavbarTextTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
