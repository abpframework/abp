namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Nav
{
    public class AbpNavTagHelper : AbpTagHelper<AbpNavTagHelper, AbpNavTagHelperService>
    {
        public AbpNavAlign Align { get; set; } = AbpNavAlign.Default;

        public NavStyle NavStyle { get; set; } = NavStyle.Default;

        public bool? Responsive { get; set; }

        public AbpNavTagHelper(AbpNavTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
