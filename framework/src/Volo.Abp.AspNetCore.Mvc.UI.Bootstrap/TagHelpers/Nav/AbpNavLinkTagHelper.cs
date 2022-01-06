using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav;

[HtmlTargetElement(Attributes = "abp-nav-link")]
public class AbpNavLinkTagHelper : AbpTagHelper<AbpNavLinkTagHelper, AbpNavLinkTagHelperService>
{
    public bool? Active { get; set; }

    public bool? Disabled { get; set; }

    public AbpNavLinkTagHelper(AbpNavLinkTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
