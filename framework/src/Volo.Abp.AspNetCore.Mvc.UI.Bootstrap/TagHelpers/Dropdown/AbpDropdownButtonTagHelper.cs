using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Dropdown
{
    public class AbpDropdownButtonTagHelper : AbpTagHelper<AbpDropdownButtonTagHelper, AbpDropdownButtonTagHelperService>
    {
        public string Text { get; set; }

        public AbpButtonSize Size { get; set; } = AbpButtonSize.Default;

        public DropdownStyle DropdownStyle { get; set; } = DropdownStyle.Single;

        public AbpButtonType ButtonType { get; set; } = AbpButtonType.Default;

        public string Icon { get; set; }

        public FontIconType IconType { get; set; } = FontIconType.FontAwesome;

        public bool? Link { get; set; }

        public bool? NavLink { get; set; }

        public AbpDropdownButtonTagHelper(AbpDropdownButtonTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
