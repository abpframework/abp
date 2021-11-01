namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Dropdown
{
    public class AbpDropdownMenuTagHelper : AbpTagHelper<AbpDropdownMenuTagHelper, AbpDropdownMenuTagHelperService>
    {
            public DropdownAlign Align { get; set; } = DropdownAlign.Start;

        public AbpDropdownMenuTagHelper(AbpDropdownMenuTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
