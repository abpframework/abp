using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;

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
            var html = GetFormInputGroupAsHtml(context, output);

            var order = GetInputOrder(TagHelper.AspFor.ModelExplorer);

            AddGroupToFormGroupContents(context, TagHelper.AspFor.Name, html, order);

            output.SuppressOutput();
        }

        protected virtual string GetFormInputGroupAsHtml(TagHelperContext context, TagHelperOutput output)
        {
            var inputTag = GetInputTag(context, out var isCheckbox);
            var inputHtml = RenderTagHelperOutput(inputTag, _encoder);
            var label = GetLabelAsHtml(inputTag, isCheckbox);

            var validation = isCheckbox ? "" : GetValidationAsHtml(context);

            return GetContent(label, inputHtml, validation, isCheckbox);
        }
        protected virtual string GetValidationAsHtml(TagHelperContext context)
        {
            var validationMessageTagHelper = new ValidationMessageTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            var attributeList = new TagHelperAttributeList { { "class", "text-danger" } };

            return RenderTagHelper(attributeList, context, validationMessageTagHelper, _encoder, "span", TagMode.StartTagAndEndTag, true);
        }

        protected virtual string GetContent(string label, string inputHtml, string validation, bool isCheckbox)
        {
            var innerContent = isCheckbox ?
                inputHtml + Environment.NewLine + label :
                label + Environment.NewLine + inputHtml;

            return "<div class=\"" + (isCheckbox ? "form-check" : "form-group") + "\">" +
                   Environment.NewLine + innerContent + Environment.NewLine +
                   Environment.NewLine + validation + Environment.NewLine +
                   "</div>";
        }

        protected virtual TagHelperOutput GetInputTag(TagHelperContext context, out bool isCheckbox)
        {
            var inputTagHelper = new InputTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            var inputTagHelperOutput = GetInnerTagHelper(new TagHelperAttributeList(), context, inputTagHelper, "input");
            isCheckbox = IsInputCheckbox(inputTagHelperOutput.Attributes);

            inputTagHelperOutput.Attributes.Add("class", isCheckbox ? "form-check-input" : "form-control");

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

            var checkboxClass = isCheckbox ? "class=\"form-check-label\" " : "";

            return "<label " + checkboxClass + GetIdAttributeAsString(inputTag) + ">"
                   + _localizer[GetLabelValue()] +
                   "</label>";
        }

        protected virtual string GetLabelValue()
        {
            return string.IsNullOrEmpty(TagHelper.Label) ?
                TagHelper.AspFor.Metadata.DisplayName :
                TagHelper.Label;
        }
    }
}