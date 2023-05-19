using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Dropdown;

public class AbpDropdownHeaderTagHelperService : AbpTagHelperService<AbpDropdownHeaderTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "h6";
        output.Attributes.AddClass("dropdown-header");
        output.TagMode = TagMode.StartTagAndEndTag;
    }
}
