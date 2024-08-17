using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse;

[HtmlTargetElement("abp-button", Attributes = "abp-collapse-id")]
[HtmlTargetElement("a", Attributes = "abp-collapse-id")]
public class AbpCollapseButtonTagHelper : AbpTagHelper<AbpCollapseButtonTagHelper, AbpCollapseButtonTagHelperService>
{
    [HtmlAttributeName("abp-collapse-id")]
    public string BodyId { get; set; } = default!;

    public AbpCollapseButtonTagHelper(AbpCollapseButtonTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
