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
            output.Attributes.AddClass("col");

            SetBreakpoint(output);
        }

        protected virtual void SetBreakpoint(TagHelperOutput output)
        {
            if (!string.IsNullOrWhiteSpace(TagHelper.Breakpoint))
            {
                output.Attributes.AddClass("col-"+TagHelper.Breakpoint);
            }
        }
    }
}