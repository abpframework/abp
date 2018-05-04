using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpInputTagHelper : AbpTagHelper<AbpInputTagHelper, AbpInputTagHelperService>
    {
        public ModelExpression AspFor { get; set; }

        public string Label { get; set; }

        [HtmlAttributeName("disabled")]
        public bool IsDisabled { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public AbpInputTagHelper(AbpInputTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
