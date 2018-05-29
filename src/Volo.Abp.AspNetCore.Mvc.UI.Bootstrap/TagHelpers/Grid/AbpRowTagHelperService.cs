using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid
{
    public class AbpRowTagHelperService : AbpTagHelperService<AbpRowTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("row");

            ProcessVerticalAlign(output);
            ProcessHorizontalAlign(output);
        }

        protected virtual void ProcessVerticalAlign(TagHelperOutput output)
        {
            if (TagHelper.VAlign == VerticalAlign.Default)
            {
                return;
            }

            output.Attributes.AddClass("align-items-" + TagHelper.VAlign.ToString().ToLowerInvariant());
        }

        protected virtual void ProcessHorizontalAlign(TagHelperOutput output)
        {
            if (TagHelper.HAlign == HorizontalAlign.Default)
            {
                return;
            }

            output.Attributes.AddClass("justify-content-" + TagHelper.HAlign.ToString().ToLowerInvariant());
        }
    }
}