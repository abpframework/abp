using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpValidationAttributeTagHelperService : AbpTagHelperService<AbpValidationAttributeTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("class","text-danger");
        }
    }
}