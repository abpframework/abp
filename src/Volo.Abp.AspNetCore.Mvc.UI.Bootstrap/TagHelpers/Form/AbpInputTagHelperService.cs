using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpInputTagHelperService : AbpTagHelperService<AbpInputTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;
        private readonly IStringLocalizer<AbpUiResource> _localizer;

        public AbpInputTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IStringLocalizer<AbpUiResource> localizer)
        {
            _generator = generator;
            _encoder = encoder;
            _localizer = localizer;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            
            var inputTag = GetInputTag();
            var inputHtml = RenderTagHelperOutput(inputTag, _encoder);
            var isCheckbox = IsInputCheckbox(inputTag.Attributes);

            var label = GetLabelAsHtml(inputTag, isCheckbox);

            var content = isCheckbox ?
                inputHtml + Environment.NewLine + label :
                label + Environment.NewLine + inputHtml;

            output.Content.SetHtmlContent(content);

            SetDivAttributes(output, isCheckbox);
        }
        
        protected virtual void SetDivAttributes(TagHelperOutput output, bool isCheckbox)
        {
            output.Attributes.RemoveAll("asp-for");

            output.Attributes.Add("class", isCheckbox ? " form-check" : "form-group");
        }

        protected virtual TagHelperOutput GetInputTag()
        {
            var inputTagHelper = new InputTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            var inputTagHelperOutput = GetInnerTagHelper(new TagHelperAttributeList(), inputTagHelper, "input"); ;
            
            inputTagHelperOutput.Attributes.Add("class",
                IsInputCheckbox(inputTagHelperOutput.Attributes) ? "form-check-input" : "form-control");

            return inputTagHelperOutput;
        }

        protected virtual bool IsInputCheckbox(TagHelperAttributeList attributes)
        {
            return attributes.Any(a => a.Value != null && a.Name == "type" && a.Value.ToString() == "checkbox");
        }

        protected virtual string GetLabelAsHtml(TagHelperOutput inputTag, bool isCheckbox)
        {
            if (string.IsNullOrEmpty(TagHelper.Label) && string.IsNullOrEmpty(TagHelper.AspFor.Metadata.DisplayName))
            {
                return "";
            }

            var label = GetLabelText();
            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");
            var idAttrAsString = "";

            if (idAttr != null)
            {
                idAttrAsString = "for=\"" + idAttr.Value + "\"";
            }

            var checkboxClass = isCheckbox ? "class=\"form-check-label\" " : "";

            return "<label " + checkboxClass + idAttrAsString + ">" + _localizer[label] + "</label>";
        }

        protected virtual string GetLabelText()
        {
            return string.IsNullOrEmpty(TagHelper.Label) ?
                TagHelper.AspFor.Metadata.DisplayName :
                TagHelper.Label;
        }
    }
}