using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [HtmlTargetElement("form", Attributes = "abp-dynamic-form", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpDynamicFormTagHelper : AbpTagHelper<AbpDynamicFormTagHelper, AbpDynamicFormTagHelperService>
    {
        [HtmlAttributeName("abp-dynamic-form")]
        public ModelExpression Model { get; set; }
        
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public AbpDynamicFormTagHelper(AbpDynamicFormTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
