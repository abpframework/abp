using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpInputTagHelper : AbpTagHelper<AbpInputTagHelper, AbpInputTagHelperService>
    {
        public ModelExpression AspFor { get; set; }

        public string Label { get; set; }

        [HtmlAttributeName("info")]
        public string InfoText { get; set; }

        [HtmlAttributeName("disabled")]
        public bool IsDisabled { get; set; } = false;

        [HtmlAttributeName("readonly")]
        public AbpReadonlyInputType IsReadonly { get; set; } = AbpReadonlyInputType.False;

        public bool AutoFocus { get; set; }

        public AbpFormControlSize Size { get; set; } = AbpFormControlSize.Default;

        [HtmlAttributeNotBound]
        public bool DisplayRequiredSymbol { get; set; } = true;

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public AbpInputTagHelper(AbpInputTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
