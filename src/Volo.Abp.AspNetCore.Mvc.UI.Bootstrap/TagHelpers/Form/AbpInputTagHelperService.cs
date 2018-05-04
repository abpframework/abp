using System;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpInputTagHelperService : AbpTagHelperService<AbpInputTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;

        public AbpInputTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder)
        {
            _generator = generator;
            _encoder = encoder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var innerHtml = GetFormInputGroupAsHtml(context, output, out var isCheckbox);

            var order = GetInputOrder(TagHelper.AspFor.ModelExplorer);

            AddGroupToFormGroupContents(
                context,
                TagHelper.AspFor.Name,
                SurroundInnerHtmlAndGet(context, output, innerHtml, isCheckbox),
                order,
                out var surpress
            );

            if (surpress)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagMode = TagMode.StartTagAndEndTag;
                output.TagName = "div";
                output.Attributes.AddClass(isCheckbox ? "form-check" : "form-group");
                output.Content.SetHtmlContent(output.Content.GetContent() + innerHtml);
            }
        }

        protected virtual string GetFormInputGroupAsHtml(TagHelperContext context, TagHelperOutput output, out bool isCheckbox)
        {
            var inputTag = GetInputTag(context, output, out isCheckbox);
            var inputHtml = RenderTagHelperOutput(inputTag, _encoder);
            var label = GetLabelAsHtml(context, output, isCheckbox);

            var validation = isCheckbox ? "" : GetValidationAsHtml(context, output);

            return GetContent(context, output, label, inputHtml, validation, isCheckbox);
        }
        protected virtual string GetValidationAsHtml(TagHelperContext context, TagHelperOutput output)
        {
            var validationMessageTagHelper = new ValidationMessageTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            var attributeList = new TagHelperAttributeList { { "class", "text-danger" } };

            return RenderTagHelper(attributeList, context, validationMessageTagHelper, _encoder, "span", TagMode.StartTagAndEndTag, true);
        }

        protected virtual string GetContent(TagHelperContext context, TagHelperOutput output, string label, string inputHtml, string validation, bool isCheckbox)
        {
            var innerContent = isCheckbox ?
                inputHtml + Environment.NewLine + label :
                label + Environment.NewLine + inputHtml;

            return Environment.NewLine + innerContent + Environment.NewLine +
                Environment.NewLine + validation + Environment.NewLine;
        }

        protected virtual string SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string innerHtml, bool isCheckbox)
        {
            return "<div class=\"" + (isCheckbox ? "form-check" : "form-group") + "\">" +
                   Environment.NewLine + innerHtml + Environment.NewLine +
                   "</div>";
        }

        protected virtual TagHelperOutput GetInputTag(TagHelperContext context, TagHelperOutput output, out bool isCheckbox)
        {
            var inputTagHelper = new InputTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            var inputTagHelperOutput = GetInnerTagHelper(new TagHelperAttributeList(), context, inputTagHelper, "input");

            if (!inputTagHelperOutput.Attributes.ContainsName("disabled") && TagHelper.IsDisabled)
            {
                inputTagHelperOutput.Attributes.Add("disabled", "true");
            }

            isCheckbox = IsInputCheckbox(context, output, inputTagHelperOutput.Attributes);
            inputTagHelperOutput.Attributes.AddClass(isCheckbox ? "form-check-input" : "form-control");

            return inputTagHelperOutput;
        }

        protected virtual bool IsInputCheckbox(TagHelperContext context, TagHelperOutput output, TagHelperAttributeList attributes)
        {
            return attributes.Any(a => a.Value != null && a.Name == "type" && a.Value.ToString() == "checkbox");
        }

        protected virtual string GetLabelAsHtml(TagHelperContext context, TagHelperOutput output, bool isCheckbox)
        {
            var labelTagHelper = new LabelTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            var attributeList = new TagHelperAttributeList();

            if (isCheckbox)
            {
                attributeList.AddClass("form-check-label");
            }

            return RenderTagHelper(attributeList, context, labelTagHelper, _encoder, "span", TagMode.StartTagAndEndTag, true);
        }
    }
}