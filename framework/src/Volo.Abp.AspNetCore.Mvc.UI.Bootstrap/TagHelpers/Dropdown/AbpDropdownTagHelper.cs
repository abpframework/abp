namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Dropdown;

public class AbpDropdownTagHelper : AbpTagHelper<AbpDropdownTagHelper, AbpDropdownTagHelperService>
{
    public DropdownDirection Direction { get; set; } = DropdownDirection.Down;

    public AbpDropdownTagHelper(AbpDropdownTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
