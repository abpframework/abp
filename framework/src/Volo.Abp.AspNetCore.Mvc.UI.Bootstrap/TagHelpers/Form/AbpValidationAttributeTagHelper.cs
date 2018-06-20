using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [HtmlTargetElement(Attributes = "asp-validation-for")]
    [HtmlTargetElement(Attributes = "asp-validation-summary")]
    public class AbpValidationAttributeTagHelper : AbpTagHelper<AbpValidationAttributeTagHelper, AbpValidationAttributeTagHelperService>, ITransientDependency
    {
        public AbpValidationAttributeTagHelper(AbpValidationAttributeTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
