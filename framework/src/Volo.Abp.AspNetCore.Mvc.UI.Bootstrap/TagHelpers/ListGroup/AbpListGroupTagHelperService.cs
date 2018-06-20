using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.ListGroup
{
    public class AbpListGroupTagHelperService : AbpTagHelperService<AbpListGroupTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.AddClass("list-group");

            if (TagHelper.Flush ?? false)
            {
                output.Attributes.AddClass("list-group-flush");
            }
        }
    }
}