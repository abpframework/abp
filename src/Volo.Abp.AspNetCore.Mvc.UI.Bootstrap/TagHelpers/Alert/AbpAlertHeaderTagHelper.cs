using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Alert
{
    [HtmlTargetElement("h1", Attributes = "abp-alert-header", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("h2", Attributes = "abp-alert-header", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("h3", Attributes = "abp-alert-header", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("h4", Attributes = "abp-alert-header", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("h5", Attributes = "abp-alert-header", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("h6", Attributes = "abp-alert-header", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpAlertHeaderTagHelper : AbpTagHelper<AbpAlertHeaderTagHelper, AbpAlertHeaderTagHelperService>
    {

        public AbpAlertHeaderTagHelper(AbpAlertHeaderTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
