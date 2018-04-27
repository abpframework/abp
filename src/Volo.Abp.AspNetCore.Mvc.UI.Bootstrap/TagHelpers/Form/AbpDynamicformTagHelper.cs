using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpDynamicFormTagHelper : AbpTagHelper<AbpDynamicFormTagHelper, AbpDynamicFormTagHelperService>
    {
        public ModelExpression Model { get; set; }

        public AbpDynamicFormTagHelper(AbpDynamicFormTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
