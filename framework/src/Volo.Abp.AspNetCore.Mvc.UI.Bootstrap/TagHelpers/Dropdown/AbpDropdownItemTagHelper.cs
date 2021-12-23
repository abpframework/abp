namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Dropdown;

public class AbpDropdownItemTagHelper : AbpTagHelper<AbpDropdownItemTagHelper, AbpDropdownItemTagHelperService>
{
    public bool? Active { get; set; }

    public bool? Disabled { get; set; }

    public AbpDropdownItemTagHelper(AbpDropdownItemTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
