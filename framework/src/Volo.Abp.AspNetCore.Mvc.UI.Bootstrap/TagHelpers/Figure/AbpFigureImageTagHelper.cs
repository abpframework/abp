using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Figure
{
    [HtmlTargetElement("abp-image", ParentTag = "abp-figure")]
    public class AbpFigureImageTagHelper : AbpTagHelper<AbpFigureImageTagHelper, AbpFigureImageTagHelperService>
    {
        public AbpFigureImageTagHelper(AbpFigureImageTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
