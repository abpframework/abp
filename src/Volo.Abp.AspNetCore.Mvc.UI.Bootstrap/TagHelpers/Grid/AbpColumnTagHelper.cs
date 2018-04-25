namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid
{
    public class AbpColumnTagHelper : AbpTagHelper<AbpColumnTagHelper, AbpColumnTagHelperService>
    {
        public string Size { get; set; }

        public VerticalAlign VAlign { get; set; } = VerticalAlign.Default;

        public AbpColumnTagHelper(AbpColumnTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
