namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavBarTagHelper : AbpTagHelper<AbpNavBarTagHelper, AbpNavBarTagHelperService>
    {
        public AbpNavbarSize Size { get; set; } = AbpNavbarSize.Default;

        public AbpNavBarTagHelper(AbpNavBarTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
