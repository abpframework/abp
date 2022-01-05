using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Popover;

[HtmlTargetElement("button", Attributes = "abp-popover")]
[HtmlTargetElement("button", Attributes = "abp-popover-right")]
[HtmlTargetElement("button", Attributes = "abp-popover-left")]
[HtmlTargetElement("button", Attributes = "abp-popover-top")]
[HtmlTargetElement("button", Attributes = "abp-popover-bottom")]
[HtmlTargetElement("abp-button", Attributes = "abp-popover")]
[HtmlTargetElement("abp-button", Attributes = "abp-popover-right")]
[HtmlTargetElement("abp-button", Attributes = "abp-popover-left")]
[HtmlTargetElement("abp-button", Attributes = "abp-popover-top")]
[HtmlTargetElement("abp-button", Attributes = "abp-popover-bottom")]
public class AbpPopoverTagHelper : AbpTagHelper<AbpPopoverTagHelper, AbpPopoverTagHelperService>
{
    public bool? Disabled { get; set; }

    public bool? Dismissible { get; set; }

    public bool? Hoverable { get; set; }

    public string AbpPopover { get; set; }

    public string AbpPopoverRight { get; set; }

    public string AbpPopoverLeft { get; set; }

    public string AbpPopoverTop { get; set; }

    public string AbpPopoverBottom { get; set; }

    public AbpPopoverTagHelper(AbpPopoverTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
