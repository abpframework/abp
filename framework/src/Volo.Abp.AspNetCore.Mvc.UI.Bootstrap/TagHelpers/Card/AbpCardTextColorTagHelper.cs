using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

[HtmlTargetElement("abp-card", Attributes = "text-color")]
[HtmlTargetElement("abp-card-header", Attributes = "text-color")]
[HtmlTargetElement("abp-card-body", Attributes = "text-color")]
[HtmlTargetElement("abp-card-footer", Attributes = "text-color")]
public class AbpCardTextColorTagHelper : AbpTagHelper<AbpCardTextColorTagHelper, AbpCardTextColorTagHelperService>
{
    public AbpCardTextColorType TextColor { get; set; } = AbpCardTextColorType.Default;

    public AbpCardTextColorTagHelper(AbpCardTextColorTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
