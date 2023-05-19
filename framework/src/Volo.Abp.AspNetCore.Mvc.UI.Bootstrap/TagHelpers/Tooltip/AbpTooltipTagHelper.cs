using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tooltip;

[HtmlTargetElement("button", Attributes = "abp-tooltip")]
[HtmlTargetElement("button", Attributes = "abp-tooltip-right")]
[HtmlTargetElement("button", Attributes = "abp-tooltip-left")]
[HtmlTargetElement("button", Attributes = "abp-tooltip-top")]
[HtmlTargetElement("button", Attributes = "abp-tooltip-bottom")]
[HtmlTargetElement("abp-button", Attributes = "abp-tooltip")]
[HtmlTargetElement("abp-button", Attributes = "abp-tooltip-right")]
[HtmlTargetElement("abp-button", Attributes = "abp-tooltip-left")]
[HtmlTargetElement("abp-button", Attributes = "abp-tooltip-top")]
[HtmlTargetElement("abp-button", Attributes = "abp-tooltip-bottom")]
public class AbpTooltipTagHelper : AbpTagHelper<AbpTooltipTagHelper, AbpTooltipTagHelperService>
{
    public string AbpTooltip { get; set; }

    public string AbpTooltipRight { get; set; }

    public string AbpTooltipLeft { get; set; }

    public string AbpTooltipTop { get; set; }

    public string AbpTooltipBottom { get; set; }

    public string Title { get; set; }

    public AbpTooltipTagHelper(AbpTooltipTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
