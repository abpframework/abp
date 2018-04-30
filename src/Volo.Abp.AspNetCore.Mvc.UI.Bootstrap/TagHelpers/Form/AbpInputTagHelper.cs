using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpInputTagHelper : AbpTagHelper<AbpInputTagHelper, AbpInputTagHelperService>, ITransientDependency
    {
        public ModelExpression AspFor { get; set; }

        public string Label { get; set; }

        [HtmlAttributeName("order")]
        public int? FieldOrder { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public AbpInputTagHelper(AbpInputTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
