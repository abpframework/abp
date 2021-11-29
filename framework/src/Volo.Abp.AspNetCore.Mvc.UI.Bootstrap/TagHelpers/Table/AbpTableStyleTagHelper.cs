using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Table
{
    [HtmlTargetElement("tr")]
    [HtmlTargetElement("td")]
    public class AbpTableStyleTagHelper : AbpTagHelper<AbpTableStyleTagHelper, AbpTableStyleTagHelperService>
    {
        public AbpTableStyle TableStyle { get; set; } = AbpTableStyle.Default;

        public AbpTableStyleTagHelper(AbpTableStyleTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
