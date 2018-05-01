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

            var order = GetInputOrder();

            AddGroupToFormGroupContents(GetFormGroupContentsList(context), html, order);

            output.SuppressOutput();
        }

        protected virtual int GetInputOrder()
        {
            return GetAttribute<DisplayOrder>(TagHelper.AspFor.ModelExplorer)?.Number ?? 0;
        }

        protected virtual void AddGroupToFormGroupContents(List<FormGroupContent> list, string html, int order)
        {
            if (list != null && !list.Any(igc => igc.Html.Contains("id=\"" + TagHelper.AspFor.Name.Replace('.', '_') + "\"")))
            {
                list.Add(new FormGroupContent
                {
                    Html = html,
                    Order = order
                });
            }
        }

        protected virtual string GetFormInputGroupAsHtml(TagHelperContext context, TagHelperOutput output)
        {
            var inputTag = GetInputTag(context, out var isCheckbox);
            var inputHtml = RenderTagHelperOutput(inputTag, _encoder);
            var label = GetLabelAsHtml(inputTag, isCheckbox);

            return GetContent(label, inputHtml, isCheckbox);
        }

        protected virtual string GetContent(string label, string inputHtml, bool isCheckbox)
        {
            var innerContent = isCheckbox ?
                inputHtml + Environment.NewLine + label :
                label + Environment.NewLine + inputHtml;

            return "<div class=\"" + (isCheckbox ? "form-check" : "form-group") + "\">" + Environment.NewLine + innerContent + Environment.NewLine + "</div>";
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

            var label = GetLabelValue();
            var checkboxClass = isCheckbox ? "class=\"form-check-label\" " : "";

            return "<label " + checkboxClass + GetIdAttributeAsString(inputTag) + ">"
                   + _localizer[label] +
                   "</label>";
        }

        protected virtual string GetLabelValue()
        {
            return string.IsNullOrEmpty(TagHelper.Label) ?
                TagHelper.AspFor.Metadata.DisplayName :
                TagHelper.Label;
        }

        protected virtual string GetIdAttributeAsString(TagHelperOutput inputTag)
        {
            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

            return idAttr != null ? "for=\"" + idAttr.Value + "\"" : "";
        }
    }
}