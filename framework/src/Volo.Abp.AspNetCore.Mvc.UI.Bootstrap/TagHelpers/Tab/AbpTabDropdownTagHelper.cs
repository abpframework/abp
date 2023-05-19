using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab;

[HtmlTargetElement("abp-tab-dropdown")]
public class AbpTabDropdownTagHelper : AbpTagHelper<AbpTabDropdownTagHelper, AbpTabDropdownTagHelperService>
{
    public string Name { get; set; }

    public string Title { get; set; }

    public AbpTabDropdownTagHelper(AbpTabDropdownTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
