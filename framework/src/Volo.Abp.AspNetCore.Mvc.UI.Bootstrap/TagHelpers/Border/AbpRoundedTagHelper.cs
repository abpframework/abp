using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Border;

[HtmlTargetElement(Attributes = "abp-rounded")]
public class AbpRoundedTagHelper : AbpTagHelper<AbpRoundedTagHelper, AbpRoundedTagHelperService>
{
    public AbpRoundedType AbpRounded { get; set; } = AbpRoundedType.Default;

    public AbpRoundedTagHelper(AbpRoundedTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
