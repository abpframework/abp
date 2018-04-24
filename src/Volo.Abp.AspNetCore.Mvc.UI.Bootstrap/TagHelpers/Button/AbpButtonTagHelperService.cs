using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    public class AbpButtonTagHelperService : AbpTagHelperService<AbpButtonTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            output.Attributes.Add("type", "button");
            output.Attributes.AddClass("btn");

            if (TagHelper.ButtonType != AbpButtonType.Default)
            {
                output.Attributes.AddClass("btn-" + TagHelper.ButtonType.ToString().ToLowerInvariant());
            }
        }
    }
}