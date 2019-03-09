using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpSelectTagHelper : AbpTagHelper<AbpSelectTagHelper, AbpSelectTagHelperService>
    {
        public ModelExpression AspFor { get; set; }

        public string Label { get; set; }

        public IEnumerable<SelectListItem> AspItems { get; set; }

        public AbpFormControlSize Size { get; set; } = AbpFormControlSize.Default;

        [HtmlAttributeName("info")]
        public string InfoText { get; set; }

        [HtmlAttributeName("required-symbol")]
        public bool DisplayRequiredSymbol { get; set; } = true;

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public AbpSelectTagHelper(AbpSelectTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
