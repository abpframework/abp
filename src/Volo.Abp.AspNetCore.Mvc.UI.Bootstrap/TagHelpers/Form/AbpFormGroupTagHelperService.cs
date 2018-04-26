using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpFormGroupTagHelperService : AbpTagHelperService<AbpFormGroupTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("form-group");
            if (TagHelper.Checkbox)
            {
                 output.Attributes.AddClass("form-check");
            }
        }
    }
}