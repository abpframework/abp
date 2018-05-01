using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [HtmlTargetElement(Attributes = "asp-validation-for")]
    public class AbpValidationAttributeTagHelper : AbpTagHelper<AbpValidationAttributeTagHelper, AbpValidationAttributeTagHelperService>, ITransientDependency
    {
        public AbpValidationAttributeTagHelper(AbpValidationAttributeTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
