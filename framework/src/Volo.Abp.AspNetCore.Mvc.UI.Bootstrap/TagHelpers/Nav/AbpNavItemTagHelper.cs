namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavItemTagHelper : AbpTagHelper<AbpNavItemTagHelper, AbpNavItemTagHelperService>
    {
        public bool? Active { get; set; }

        public bool? Dropdown { get; set; }

        public AbpNavItemTagHelper(AbpNavItemTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
