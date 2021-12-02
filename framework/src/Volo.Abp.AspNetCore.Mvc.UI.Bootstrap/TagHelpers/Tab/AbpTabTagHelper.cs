using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab;

[HtmlTargetElement("abp-tab")]
public class AbpTabTagHelper : AbpTagHelper<AbpTabTagHelper, AbpTabTagHelperService>
{
    public string Name { get; set; }

    public string Title { get; set; }

    public bool? Active { get; set; }

    public string ParentDropdownName { get; set; }

    public AbpTabTagHelper(AbpTabTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
