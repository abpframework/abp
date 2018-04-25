using System;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid
{
    public class AbpColumnTagHelperService : AbpTagHelperService<AbpColumnTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            ProcessColClass(output);
        }

        protected virtual void ProcessColClass(TagHelperOutput output)
        {
            output.Attributes.AddClass("col" + (string.IsNullOrWhiteSpace(TagHelper.Size)? "" : "-" + TagHelper.Size));
        }

        protected virtual void ProcessVerticalAlign(TagHelperOutput output)
        {
            if (TagHelper.VAlign == VerticalAlign.Default)
            {
                return;
            }

            output.Attributes.AddClass("align-self-" + TagHelper.VAlign.ToString().ToLowerInvariant());
        }
    }
}