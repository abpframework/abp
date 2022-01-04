using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

[HtmlTargetElement("a", Attributes = "abp-card-link")]
public class AbpCardLinkTagHelper : AbpTagHelper<AbpCardLinkTagHelper, AbpCardLinkTagHelperService>
{
    public AbpCardLinkTagHelper(AbpCardLinkTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
