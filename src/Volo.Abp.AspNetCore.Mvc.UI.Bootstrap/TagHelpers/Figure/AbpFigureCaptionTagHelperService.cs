using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Figure
{
    public class AbpFigureCaptionTagHelperService : AbpTagHelperService<AbpFigureCaptionTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "figcaption";
            output.Attributes.AddClass("figure-caption");
        }
    }
}