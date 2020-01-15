﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpInputTagHelperService : AbpTagHelperService<AbpInputTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;
        private readonly IAbpTagHelperLocalizer _tagHelperLocalizer;

        public AbpInputTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IAbpTagHelperLocalizer tagHelperLocalizer)
        {
            _generator = generator;
            _encoder = encoder;
            _tagHelperLocalizer = tagHelperLocalizer;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var (innerHtml, isCheckBox) = await GetFormInputGroupAsHtmlAsync(context, output).ConfigureAwait(false);

            var order = TagHelper.AspFor.ModelExplorer.GetDisplayOrder();

            AddGroupToFormGroupContents(
                context,
                TagHelper.AspFor.Name,
                SurroundInnerHtmlAndGet(context, output, innerHtml, isCheckBox),
                order,
                out var suppress
            );

            if (suppress)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagMode = TagMode.StartTagAndEndTag;
                output.TagName = "div";
                LeaveOnlyGroupAttributes(context, output);
                output.Attributes.AddClass(isCheckBox ? "custom-checkbox" : "form-group");
                output.Attributes.AddClass(isCheckBox ? "custom-control" : "");
                output.Attributes.AddClass(isCheckBox ? "mb-2" : "");
                output.Content.SetHtmlContent(output.Content.GetContent() + innerHtml);
            }
        }

        protected virtual async Task<(string, bool)> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
        {
            var (inputTag, isCheckBox) = await GetInputTagHelperOutputAsync(context, output).ConfigureAwait(false);
            
            var inputHtml = inputTag.Render(_encoder);
            var label = await GetLabelAsHtmlAsync(context, output, inputTag, isCheckBox).ConfigureAwait(false);
            var info = GetInfoAsHtml(context, output, inputTag, isCheckBox);
            var validation = isCheckBox ? "" : await GetValidationAsHtmlAsync(context, output, inputTag).ConfigureAwait(false);

            return (GetContent(context, output, label, inputHtml, validation, info, isCheckBox), isCheckBox);
        }

        protected virtual async Task<string> GetValidationAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
        {
            if (IsOutputHidden(inputTag))
            {
                return "";
            }

            var validationMessageTagHelper = new ValidationMessageTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            var attributeList = new TagHelperAttributeList { { "class", "text-danger" } };

            return await validationMessageTagHelper.RenderAsync(attributeList, context, _encoder, "span", TagMode.StartTagAndEndTag).ConfigureAwait(false);
        }

        protected virtual string GetContent(TagHelperContext context, TagHelperOutput output, string label, string inputHtml, string validation, string infoHtml, bool isCheckbox)
        {
            var innerContent = isCheckbox ?
                inputHtml + label :
                label + inputHtml;

            return  innerContent + infoHtml + validation;
        }

        protected virtual string SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string innerHtml, bool isCheckbox)
        {
            return "<div class=\"" + (isCheckbox ? "custom-checkbox custom-control" : "form-group") + "\">" +
                   Environment.NewLine + innerHtml + Environment.NewLine +
                   "</div>";
        }

        protected virtual TagHelper GetInputTagHelper(TagHelperContext context, TagHelperOutput output)
        {
            var textAreaAttribute = TagHelper.AspFor.ModelExplorer.GetAttribute<TextArea>();

            if (textAreaAttribute != null)
            {
                return new TextAreaTagHelper(_generator)
                {
                    For = TagHelper.AspFor,
                    ViewContext = TagHelper.ViewContext
                };
            }

            return new InputTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };
        }

        protected virtual async Task<(TagHelperOutput, bool)> GetInputTagHelperOutputAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tagHelper = GetInputTagHelper(context, output);

            var inputTagHelperOutput = await tagHelper.ProcessAndGetOutputAsync(GetInputAttributes(context, output), context, "input").ConfigureAwait(false);

            ConvertToTextAreaIfTextArea(inputTagHelperOutput);
            AddDisabledAttribute(inputTagHelperOutput);
            AddAutoFocusAttribute(inputTagHelperOutput);
            var isCheckbox = IsInputCheckbox(context, output, inputTagHelperOutput.Attributes);
            AddFormControlClass(context, output, isCheckbox, inputTagHelperOutput);
            AddReadOnlyAttribute(inputTagHelperOutput);
            AddPlaceholderAttribute(inputTagHelperOutput);
            AddInfoTextId(inputTagHelperOutput);

            return (inputTagHelperOutput, isCheckbox);
        }

        private void AddFormControlClass(TagHelperContext context, TagHelperOutput output, bool isCheckbox, TagHelperOutput inputTagHelperOutput)
        {
            var className = "form-control";

            if (isCheckbox)
            {
                className = "custom-control-input";
            }

            inputTagHelperOutput.Attributes.AddClass(className + " " + GetSize(context, output));
        }

        protected virtual void AddAutoFocusAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (TagHelper.AutoFocus && !inputTagHelperOutput.Attributes.ContainsName("data-auto-focus"))
            {
                inputTagHelperOutput.Attributes.Add("data-auto-focus", "true");
            }
        }

        protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (inputTagHelperOutput.Attributes.ContainsName("disabled") == false && 
                     (TagHelper.IsDisabled || TagHelper.AspFor.ModelExplorer.GetAttribute<DisabledInput>() != null))
            {
                inputTagHelperOutput.Attributes.Add("disabled", "");
            }
        }

        protected virtual void AddReadOnlyAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (inputTagHelperOutput.Attributes.ContainsName("readonly") == false && 
                    (TagHelper.IsReadonly != false || TagHelper.AspFor.ModelExplorer.GetAttribute<ReadOnlyInput>() != null))
            {
                inputTagHelperOutput.Attributes.Add("readonly", "");
            }
        }

        protected virtual void AddPlaceholderAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (inputTagHelperOutput.Attributes.ContainsName("placeholder"))
            {
                return;
            }

            var attribute = TagHelper.AspFor.ModelExplorer.GetAttribute<Placeholder>();

            if (attribute != null)
            {
                var placeholderLocalized = _tagHelperLocalizer.GetLocalizedText(attribute.Value, TagHelper.AspFor.ModelExplorer);

                inputTagHelperOutput.Attributes.Add("placeholder", placeholderLocalized);
            }
        }

        protected virtual void AddInfoTextId(TagHelperOutput inputTagHelperOutput)
        {
            if (TagHelper.AspFor.ModelExplorer.GetAttribute<InputInfoText>() == null)
            {
                return;
            }

            var idAttr = inputTagHelperOutput.Attributes.FirstOrDefault(a => a.Name == "id");

            if (idAttr == null)
            {
                return;
            }

            var infoText = _tagHelperLocalizer.GetLocalizedText(idAttr.Value + "InfoText", TagHelper.AspFor.ModelExplorer);

            inputTagHelperOutput.Attributes.Add("aria-describedby", infoText);
        }

        protected virtual bool IsInputCheckbox(TagHelperContext context, TagHelperOutput output, TagHelperAttributeList attributes)
        {
            return attributes.Any(a => a.Value != null && a.Name == "type" && a.Value.ToString() == "checkbox");
        }

        protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, bool isCheckbox)
        {
            if (IsOutputHidden(inputTag))
            {
                return "";
            }

            if (string.IsNullOrEmpty(TagHelper.Label))
            {
                return await GetLabelAsHtmlUsingTagHelperAsync(context, output, isCheckbox).ConfigureAwait(false) + GetRequiredSymbol(context, output);
            }

            var checkboxClass = isCheckbox ? "class=\"custom-control-label\" " : "";

            return "<label " + checkboxClass + GetIdAttributeAsString(inputTag) + ">"
                   + TagHelper.Label +
                   "</label>" + GetRequiredSymbol(context, output);
        }

        protected virtual string GetRequiredSymbol(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.DisplayRequiredSymbol)
            {
                return "";
            }

            return TagHelper.AspFor.ModelExplorer.GetAttribute<RequiredAttribute>() != null ? "<span> * </span>":"";
        }

        protected virtual string GetInfoAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, bool isCheckbox)
        {
            if (IsOutputHidden(inputTag))
            {
                return "";
            }

            if (isCheckbox)
            {
                return "";
            }

            var text = "";

            if (!string.IsNullOrEmpty(TagHelper.InfoText))
            {
                text = TagHelper.InfoText;
            }
            else
            {
                var infoAttribute = TagHelper.AspFor.ModelExplorer.GetAttribute<InputInfoText>();
                if (infoAttribute != null)
                {
                    text = infoAttribute.Text;
                }
                else
                {
                    return "";
                }
            }

            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");
            var localizedText = _tagHelperLocalizer.GetLocalizedText(text, TagHelper.AspFor.ModelExplorer);

            return "<small id=\""+ idAttr?.Value + "InfoText\" class=\"form-text text-muted\">" +
                   localizedText +
                   "</small>";
        }

        protected virtual async Task<string> GetLabelAsHtmlUsingTagHelperAsync(TagHelperContext context, TagHelperOutput output, bool isCheckbox)
        {
            var labelTagHelper = new LabelTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            var attributeList = new TagHelperAttributeList();

            if (isCheckbox)
            {
                attributeList.AddClass("custom-control-label");
            }

            return await labelTagHelper.RenderAsync(attributeList, context, _encoder, "label", TagMode.StartTagAndEndTag).ConfigureAwait(false);
        }

        protected virtual void ConvertToTextAreaIfTextArea(TagHelperOutput tagHelperOutput)
        {
            var textAreaAttribute = TagHelper.AspFor.ModelExplorer.GetAttribute<TextArea>();

            if (textAreaAttribute == null)
            {
                return;
            }

            tagHelperOutput.TagName = "textarea";
            tagHelperOutput.TagMode = TagMode.StartTagAndEndTag;
            tagHelperOutput.Content.SetContent(TagHelper.AspFor.ModelExplorer.Model?.ToString());
            if (textAreaAttribute.Rows > 0)
            {
                tagHelperOutput.Attributes.Add("rows", textAreaAttribute.Rows);
            }
            if (textAreaAttribute.Cols > 0)
            {
                tagHelperOutput.Attributes.Add("cols", textAreaAttribute.Cols);
            }
        }

        protected virtual TagHelperAttributeList GetInputAttributes(TagHelperContext context, TagHelperOutput output)
        {
            var groupPrefix = "group-";

            var tagHelperAttributes = output.Attributes.Where(a => !a.Name.StartsWith(groupPrefix)).ToList();
            var attrList = new TagHelperAttributeList();

            foreach (var tagHelperAttribute in tagHelperAttributes)
            {
                attrList.Add(tagHelperAttribute);
            }

            return attrList;
        }

        protected virtual void LeaveOnlyGroupAttributes(TagHelperContext context, TagHelperOutput output)
        {
            var groupPrefix = "group-";
            var tagHelperAttributes = output.Attributes.Where(a => a.Name.StartsWith(groupPrefix)).ToList();

            output.Attributes.Clear();

            foreach (var tagHelperAttribute in tagHelperAttributes)
            {
                var nameWithoutPrefix = tagHelperAttribute.Name.Substring(groupPrefix.Length);
                var newAttritube = new TagHelperAttribute(nameWithoutPrefix, tagHelperAttribute.Value);
                output.Attributes.Add(newAttritube);
            }
        }

        protected virtual string GetSize(TagHelperContext context, TagHelperOutput output)
        {
            var attribute = TagHelper.AspFor.ModelExplorer.GetAttribute<FormControlSize>();

            if (attribute != null)
            {
                TagHelper.Size = attribute.Size;
            }

            switch (TagHelper.Size)
            {
                case AbpFormControlSize.Small:
                    return "form-control-sm";
                case AbpFormControlSize.Medium:
                    return "form-control-md";
                case AbpFormControlSize.Large:
                    return "form-control-lg";
            }

            return "";
        }

        protected virtual bool IsOutputHidden(TagHelperOutput inputTag)
        {
            return inputTag.Attributes.Any(a => a.Name.ToLowerInvariant() == "type" && a.Value.ToString().ToLowerInvariant() == "hidden");
        }

        protected virtual string GetIdAttributeAsString(TagHelperOutput inputTag)
        {
            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

            return idAttr != null ? "for=\"" + idAttr.Value + "\"" : "";
        }

        protected virtual void AddGroupToFormGroupContents(TagHelperContext context, string propertyName, string html, int order, out bool suppress)
        {
            var list = context.GetValue<List<FormGroupItem>>(FormGroupContents) ?? new List<FormGroupItem>();
            suppress = list == null;

            if (list != null && !list.Any(igc => igc.HtmlContent.Contains("id=\"" + propertyName.Replace('.', '_') + "\"")))
            {
                list.Add(new FormGroupItem
                {
                    HtmlContent = html,
                    Order = order
                });
            }
        }
    }
}