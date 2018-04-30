using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpDynamicFormTagHelper : AbpTagHelper<AbpDynamicFormTagHelper, AbpDynamicFormTagHelperService>
    {
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
