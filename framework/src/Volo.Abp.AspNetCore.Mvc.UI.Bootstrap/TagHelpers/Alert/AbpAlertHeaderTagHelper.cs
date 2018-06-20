using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Alert
{
    [HtmlTargetElement("h1", ParentTag = "abp-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("h2", ParentTag = "abp-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("h3", ParentTag = "abp-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("h4", ParentTag = "abp-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("h5", ParentTag = "abp-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("h6", ParentTag = "abp-alert", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpAlertHeaderTagHelper : AbpTagHelper<AbpAlertHeaderTagHelper, AbpAlertHeaderTagHelperService>
    {
        public AbpAlertHeaderTagHelper(AbpAlertHeaderTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
