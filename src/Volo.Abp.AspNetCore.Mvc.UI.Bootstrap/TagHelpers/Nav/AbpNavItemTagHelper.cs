namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavItemTagHelper : AbpTagHelper<AbpNavItemTagHelper, AbpNavItemTagHelperService>
    {
        public bool? Active { get; set; } 

        public bool? Disabled { get; set; }

        public string Href { get; set; }
        
        public AbpNavItemTagHelper(AbpNavItemTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
