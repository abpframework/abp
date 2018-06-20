using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Table
{
    [HtmlTargetElement("thead")]
    public class AbpTableHeaderTagHelper : AbpTagHelper<AbpTableHeaderTagHelper, AbpTableHeaderTagHelperService>
    {
        public AbpTableHeaderTheme Theme { get; set; } = AbpTableHeaderTheme.Default;
        
        public AbpTableHeaderTagHelper(AbpTableHeaderTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
