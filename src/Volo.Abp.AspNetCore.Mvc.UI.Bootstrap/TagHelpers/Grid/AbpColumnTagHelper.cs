using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid
{
    public class AbpColumnTagHelper : AbpTagHelper<AbpColumnTagHelper, AbpColumnTagHelperService>
    {
        public string Size { get; set; }

        [HtmlAttributeName("order")]
        public string ColumnOrder { get; set; }

        public VerticalAlign VAlign { get; set; } = VerticalAlign.Default;

        public AbpColumnTagHelper(AbpColumnTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
