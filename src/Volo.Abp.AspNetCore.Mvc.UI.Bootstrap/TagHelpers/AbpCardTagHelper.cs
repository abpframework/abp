using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public class AbpCardTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //TODO: Remove abp-modal HTML element which is unnecessary!
            output.TagName = "div";
            output.Attributes.Add("class", "card"); //TODO: Append class if any exists
        }
    }

    public class AbpCardHeaderTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //TODO: Remove abp-modal HTML element which is unnecessary!
            output.TagName = "div";
            output.Attributes.Add("class", "card-header"); //TODO: Append class if any exists
        }
    }

    public class AbpCardBodyTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //TODO: Remove abp-modal HTML element which is unnecessary!
            output.TagName = "div";
            output.Attributes.Add("class", "card-body"); //TODO: Append class if any exists
        }
    }
}
