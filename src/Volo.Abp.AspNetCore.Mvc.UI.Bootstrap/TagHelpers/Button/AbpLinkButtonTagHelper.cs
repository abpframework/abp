using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button
{
    [HtmlTargetElement("a", Attributes = "abp-button")]
    [HtmlTargetElement("input", Attributes = "abp-button", TagStructure = TagStructure.WithoutEndTag)]
    public class AbpLinkButtonTagHelper : AbpTagHelper<AbpLinkButtonTagHelper, AbpLinkButtonTagHelperService>
    {
        [HtmlAttributeName("abp-button")]
        public AbpButtonType ButtonType { get; set; }

        public AbpLinkButtonTagHelper(AbpLinkButtonTagHelperService service) 
            : base(service)
        {

        }
    }
}
