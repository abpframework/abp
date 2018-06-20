using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Table
{
    [HtmlTargetElement("tr")]
    [HtmlTargetElement("td")]
    public class AbpTableStyleTagHelper : AbpTagHelper<AbpTableStyleTagHelper, AbpTableStyleTagHelperService>
    {
        public AbpTableStyle AbpTableStyle { get; set; } = AbpTableStyle.Default;

        public AbpTableStyle AbpDarkTableStyle { get; set; } = AbpTableStyle.Default;

        public AbpTableStyleTagHelper(AbpTableStyleTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
