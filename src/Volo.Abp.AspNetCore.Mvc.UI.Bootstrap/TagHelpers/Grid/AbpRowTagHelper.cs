namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid
{
    public class AbpRowTagHelper : AbpTagHelper<AbpRowTagHelper, AbpRowTagHelperService>
    {
        public VerticalAlign VAlign { get; set; } = VerticalAlign.Default;

        public HorizontalAlign HAlign { get; set; } = HorizontalAlign.Default;

        public AbpRowTagHelper(AbpRowTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
