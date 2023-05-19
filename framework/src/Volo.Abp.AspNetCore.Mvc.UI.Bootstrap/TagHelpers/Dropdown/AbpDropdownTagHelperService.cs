using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Dropdown;

public class AbpDropdownTagHelperService : AbpTagHelperService<AbpDropdownTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.AddClass("dropdown");
        output.Attributes.AddClass("btn-group");

        SetDirection(context, output);

        output.TagMode = TagMode.StartTagAndEndTag;
    }

    protected virtual void SetDirection(TagHelperContext context, TagHelperOutput output)
    {
        switch (TagHelper.Direction)
        {
            case DropdownDirection.Down:
                return;
            case DropdownDirection.Up:
                output.Attributes.AddClass("dropup");
                return;
            case DropdownDirection.Right:
                output.Attributes.AddClass("dropright");
                return;
            case DropdownDirection.Left:
                output.Attributes.AddClass("dropleft");
                return;
        }
    }
}
