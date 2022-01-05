using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Alert;

[HtmlTargetElement("a", Attributes = "abp-alert-link", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpAlertLinkTagHelper : AbpTagHelper<AbpAlertLinkTagHelper, AbpAlertLinkTagHelperService>
{
    public AbpAlertLinkTagHelper(AbpAlertLinkTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
