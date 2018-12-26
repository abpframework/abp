using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpInputTagHelperService : AbpTagHelperService<AbpInputTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;
        private readonly AbpMvcDataAnnotationsLocalizationOptions _options;

        public AbpInputTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IOptions<AbpMvcDataAnnotationsLocalizationOptions> options, IStringLocalizerFactory stringLocalizerFactory)
        {
            _generator = generator;
            _encoder = encoder;
            _stringLocalizerFactory = stringLocalizerFactory;
            _options = options.Value;
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
                LeaveOnlyGroupAttributes(context, output);
                output.Attributes.AddClass(isCheckbox ? "form-check" : "form-group");
                output.Content.SetHtmlContent(output.Content.GetContent() + innerHtml);
            }
        }

        protected virtual string GetFormInputGroupAsHtml(TagHelperContext context, TagHelperOutput output, out bool isCheckbox)
        {
            var inputTag = GetInputTagHelperOutput(context, output, out isCheckbox);

            var inputHtml = RenderTagHelperOutput(inputTag, _encoder);
            var label = GetLabelAsHtml(context, output, inputTag, isCheckbox);
            var info = GetInfoAsHtml(context, output, inputTag, isCheckbox);
            var validation = isCheckbox ? "" : GetValidationAsHtml(context, output, inputTag);

            return GetContent(context, output, label, inputHtml, validation, info, isCheckbox);
        }

        protected virtual string GetValidationAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
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

            return RenderTagHelper(attributeList, context, validationMessageTagHelper, _encoder, "span", TagMode.StartTagAndEndTag, true);
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
            return "<div class=\"" + (isCheckbox ? "form-check" : "form-group") + "\">" +
                   Environment.NewLine + innerHtml + Environment.NewLine +
                   "</div>";
        }

        protected virtual TagHelper GetInputTagHelper(TagHelperContext context, TagHelperOutput output)
        {
            var textAreaAttribute = GetAttribute<TextArea>(TagHelper.AspFor.ModelExplorer);

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

        protected virtual TagHelperOutput GetInputTagHelperOutput(TagHelperContext context, TagHelperOutput output, out bool isCheckbox)
        {
            var tagHelper = GetInputTagHelper(context, output);

            var inputTagHelperOutput = GetInnerTagHelper(GetInputAttributes(context, output), context, tagHelper, "input");

            ConvertToTextAreaIfTextArea(inputTagHelperOutput);
            AddDisabledAttribute(inputTagHelperOutput);
            AddAutoFocusAttribute(inputTagHelperOutput);
            isCheckbox = IsInputCheckbox(context, output, inputTagHelperOutput.Attributes);
            AddFormControlClass(context, output, isCheckbox, inputTagHelperOutput);
            AddReadOnlyAttribute(inputTagHelperOutput);
            AddPlaceholderAttribute(inputTagHelperOutput);
            AddInfoTextId(inputTagHelperOutput);

            return inputTagHelperOutput;
        }

        private void AddFormControlClass(TagHelperContext context, TagHelperOutput output, bool isCheckbox, TagHelperOutput inputTagHelperOutput)
        {
            var className = "form-control";
            var readonlyAttribute = GetAttribute<ReadOnlyInput>(TagHelper.AspFor.ModelExplorer);

            if (isCheckbox)
            {
                className = "form-check-input";
            }
            else if (TagHelper.IsReadonly == AbpReadonlyInputType.True_PlainText || (readonlyAttribute != null && readonlyAttribute.PlainText))
            {
                className = "form-control-plaintext";
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
                     (TagHelper.IsDisabled || GetAttribute<DisabledInput>(TagHelper.AspFor.ModelExplorer) != null))
            {
                inputTagHelperOutput.Attributes.Add("disabled", "");
            }
        }

        protected virtual void AddReadOnlyAttribute(TagHelperOutput inputTagHelperOutput)
        {
            if (inputTagHelperOutput.Attributes.ContainsName("readonly") == false && 
                    (TagHelper.IsReadonly != AbpReadonlyInputType.False || GetAttribute<ReadOnlyInput>(TagHelper.AspFor.ModelExplorer) != null))
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

            var attribute = GetAttribute<Placeholder>(TagHelper.AspFor.ModelExplorer);

            if (attribute != null)
            {
                inputTagHelperOutput.Attributes.Add("placeholder", LocalizeText(attribute.Value));
            }
        }

        protected virtual void AddInfoTextId(TagHelperOutput inputTagHelperOutput)
        {
            if (GetAttribute<InputInfoText>(TagHelper.AspFor.ModelExplorer) == null)
            {
                return;
            }

            var idAttr = inputTagHelperOutput.Attributes.FirstOrDefault(a => a.Name == "id");

            if (idAttr == null)
            {
                return;
            }

            inputTagHelperOutput.Attributes.Add("aria-describedby", LocalizeText(idAttr.Value + "InfoText"));
        }

        protected virtual string LocalizeText(string text)
        {
            IStringLocalizer localizer = null;
            var resourceType = _options.AssemblyResources.GetOrDefault(TagHelper.AspFor.ModelExplorer.Container.ModelType.Assembly);

            if (resourceType != null)
            {
               localizer = _stringLocalizerFactory.Create(resourceType);
            }

            return localizer == null? text: localizer[text].Value;
        }

        protected virtual bool IsInputCheckbox(TagHelperContext context, TagHelperOutput output, TagHelperAttributeList attributes)
        {
            return attributes.Any(a => a.Value != null && a.Name == "type" && a.Value.ToString() == "checkbox");
        }

        protected virtual string GetLabelAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag, bool isCheckbox)
        {
            if (IsOutputHidden(inputTag))
            {
                return "";
            }

            if (string.IsNullOrEmpty(TagHelper.Label))
            {
                return GetLabelAsHtmlUsingTagHelper(context, output, isCheckbox) + GetRequiredSymbol(context, output, inputTag);
            }

            var checkboxClass = isCheckbox ? "class=\"form-check-label\" " : "";

            return "<label " + checkboxClass + GetIdAttributeAsString(inputTag) + ">"
                   + TagHelper.Label +
                   "</label>" + GetRequiredSymbol(context, output, inputTag);
        }

        protected virtual string GetRequiredSymbol(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
        {
            if (!TagHelper.DisplayRequiredSymbol)
            {
                return "";
            }

            return GetAttribute<RequiredAttribute>(TagHelper.AspFor.ModelExplorer) != null ? "<span> (*) </span>":"";
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

            string text = "";

            if (!string.IsNullOrEmpty(TagHelper.InfoText))
            {
                text = TagHelper.InfoText;
            }
            else
            {
                var infoAttribute = GetAttribute<InputInfoText>(TagHelper.AspFor.ModelExplorer);
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

            return "<small id=\""+ idAttr?.Value + "InfoText\" class=\"form-text text-muted\">" +
                   LocalizeText(text) +
                   "</small>";
        }

        protected virtual string GetLabelAsHtmlUsingTagHelper(TagHelperContext context, TagHelperOutput output, bool isCheckbox)
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

            return RenderTagHelper(attributeList, context, labelTagHelper, _encoder, "label", TagMode.StartTagAndEndTag, true);
        }

        protected virtual void ConvertToTextAreaIfTextArea(TagHelperOutput tagHelperOutput)
        {
            var textAreaAttribute = GetAttribute<TextArea>(TagHelper.AspFor.ModelExplorer);

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
            var attribute = GetAttribute<FormControlSize>(TagHelper.AspFor.ModelExplorer);

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
    }
}