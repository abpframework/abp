using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpDynamicFormTagHelperService : AbpTagHelperService<AbpDynamicFormTagHelper>
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly AbpInputTagHelper _abpInputTagHelper;
        private readonly AbpSelectTagHelper _abpSelectTagHelper;

        public AbpDynamicFormTagHelperService(HtmlEncoder htmlEncoder, AbpInputTagHelper abpInputTagHelper, AbpSelectTagHelper abpSelectTagHelper)
        {
            _htmlEncoder = htmlEncoder;
            _abpInputTagHelper = abpInputTagHelper;
            _abpSelectTagHelper = abpSelectTagHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var model = TagHelper.Model;

        }

    }
}