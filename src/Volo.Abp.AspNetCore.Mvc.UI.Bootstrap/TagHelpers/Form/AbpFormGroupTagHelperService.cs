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
    public class AbpFormGroupTagHelperService : AbpTagHelperService<AbpFormGroupTagHelper>
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly AbpInputTagHelper _abpInputTagHelper;

        public AbpFormGroupTagHelperService(HtmlEncoder htmlEncoder, AbpInputTagHelper abpInputTagHelper)
        {
            _htmlEncoder = htmlEncoder;
            _abpInputTagHelper = abpInputTagHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("form-group");
            if (TagHelper.Checkbox)
            {
                output.Attributes.AddClass("form-check");
            }


            //var attributes = new TagHelperAttributeList
            //{
            //    {"Type", "Text"},
            //    {"Placeholder", "plcHlder"}
            //};
            
            //output.Content.AppendHtml(RenderInputTagHelper(attributes, _abpInputTagHelper, _htmlEncoder));

        }

    }
}