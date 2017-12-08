using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public class AbpCardHeaderTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //TODO: Remove abp-modal HTML element which is unnecessary!
            output.TagName = "div";
            output.Attributes.AddClass("card-header");
        }
    }
}