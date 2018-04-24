namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid
{
    public class AbpColumnTagHelper : AbpTagHelper<AbpColumnTagHelper, AbpColumnTagHelperService>
    {
        public string Breakpoint { get; set; }

        public AbpColumnTagHelper(AbpColumnTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
