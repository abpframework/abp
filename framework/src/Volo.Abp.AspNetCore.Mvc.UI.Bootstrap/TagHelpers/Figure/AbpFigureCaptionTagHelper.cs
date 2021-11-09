using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Figure;

[HtmlTargetElement("abp-figcaption")]
public class AbpFigureCaptionTagHelper : AbpTagHelper<AbpFigureCaptionTagHelper, AbpFigureCaptionTagHelperService>
{
    public AbpFigureCaptionTagHelper(AbpFigureCaptionTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
