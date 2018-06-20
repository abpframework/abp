using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Dropdown
{
    public class AbpDropdownDividerTagHelperService : AbpTagHelperService<AbpDropdownDividerTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("dropdown-divider");
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}