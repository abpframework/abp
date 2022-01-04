using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Border;

[HtmlTargetElement(Attributes = "abp-border")]
public class AbpBorderTagHelper : AbpTagHelper<AbpBorderTagHelper, AbpBorderTagHelperService>
{
    public AbpBorderType AbpBorder { get; set; } = AbpBorderType.Default;

    public AbpBorderTagHelper(AbpBorderTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
