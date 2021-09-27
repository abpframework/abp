using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [OutputElementHint("select")]
    public class AbpSelectTagHelper : AbpTagHelper<AbpSelectTagHelper, AbpSelectTagHelperService>
    {
        public ModelExpression AspFor { get; set; }

        public string Label { get; set; }

        public bool SuppressLabel { get; set; }

        public IEnumerable<SelectListItem> AspItems { get; set; }

        public AbpFormControlSize Size { get; set; } = AbpFormControlSize.Default;

        [HtmlAttributeName("info")]
        public string InfoText { get; set; }

        [HtmlAttributeName("required-symbol")]
        public bool DisplayRequiredSymbol { get; set; } = true;

        public string AutocompleteApiUrl { get; set; }

        public string AutocompleteItemsPropertyName { get; set; }

        public string AutocompleteDisplayPropertyName { get; set; }

        public string AutocompleteValuePropertyName { get; set; }

        public string AutocompleteFilterParamName { get; set; }

        public string AutocompleteSelectedItemName { get; set; }

        public string AutocompleteSelectedItemValue { get; set; }

        public AbpSelectTagHelper(AbpSelectTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
