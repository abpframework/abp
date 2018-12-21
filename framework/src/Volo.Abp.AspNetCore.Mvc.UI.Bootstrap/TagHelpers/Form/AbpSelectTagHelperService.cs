using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpSelectTagHelperService : AbpTagHelperService<AbpSelectTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;
        private readonly AbpMvcDataAnnotationsLocalizationOptions _options;

        public AbpSelectTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IOptions<AbpMvcDataAnnotationsLocalizationOptions> options, IStringLocalizerFactory stringLocalizerFactory)
        {
            _generator = generator;
            _encoder = encoder;
            _stringLocalizerFactory = stringLocalizerFactory;
            _options = options.Value;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var innerHtml = GetFormInputGroupAsHtml(context, output);

            var order = GetInputOrder(TagHelper.AspFor.ModelExplorer);

            AddGroupToFormGroupContents(context, TagHelper.AspFor.Name, SurroundInnerHtmlAndGet(context, output, innerHtml), order, out var surpress);

            if (surpress)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "div";
                LeaveOnlyGroupAttributes(context, output);
                output.Attributes.AddClass("form-group");
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Content.SetHtmlContent(innerHtml);
            }
        }

        protected virtual string GetFormInputGroupAsHtml(TagHelperContext context, TagHelperOutput output)
        {
            var selectTag = GetSelectTag(context, output);
            var selectAsHtml = RenderTagHelperOutput(selectTag, _encoder);
            var label = GetLabelAsHtml(context, output, selectTag);
            var validation =  GetValidationAsHtml(context, output, selectTag);

            return label + Environment.NewLine + selectAsHtml + Environment.NewLine + validation;
        }

        protected virtual string SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string innerHtml)
        {
            return "<div class=\"form-group\">" + Environment.NewLine + innerHtml + Environment.NewLine + "</div>";
        }

        protected virtual TagHelperOutput GetSelectTag(TagHelperContext context, TagHelperOutput output)
        {
            var selectTagHelper = new SelectTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                Items = GetSelectItems(context, output),
                ViewContext = TagHelper.ViewContext
            };

            var inputTagHelperOutput = GetInnerTagHelper(GetInputAttributes(context, output), context, selectTagHelper, "select", TagMode.StartTagAndEndTag);

            inputTagHelperOutput.Attributes.AddClass("form-control");
            inputTagHelperOutput.Attributes.AddClass(GetSize(context,output));
            AddDisabledAttribute(inputTagHelperOutput);

            return inputTagHelperOutput;
        }

        protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
        {
            var disabledAttribute = GetAttribute<DisabledInput>(TagHelper.AspFor.ModelExplorer);

            if (disabledAttribute != null && !inputTagHelperOutput.Attributes.ContainsName("disabled"))
            {
                inputTagHelperOutput.Attributes.Add("disabled", "");
            }
        }

        protected virtual List<SelectListItem> GetSelectItems(TagHelperContext context, TagHelperOutput output)
        {
            var selectItems = TagHelper.AspItems?.ToList();

            if (TagHelper.AspItems == null &&
                !GetSelectItemsIfProvidedByEnum(context, output, TagHelper.AspFor.ModelExplorer, out selectItems) &&
                !GetSelectItemsIfProvidedFromAttribute(context, output, TagHelper.AspFor.ModelExplorer, out selectItems))
            {
                throw new Exception("No items provided for select attribute.");
            }

            return selectItems;
        }

        protected virtual string GetLabelAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput selectTag)
        {
            if (!string.IsNullOrEmpty(TagHelper.Label))
            {
                return "<label " + GetIdAttributeAsString(selectTag) + ">" + TagHelper.Label + "</label>";
            }

            return GetLabelAsHtmlUsingTagHelper(context, output);
        }

        protected virtual bool GetSelectItemsIfProvidedByEnum(TagHelperContext context, TagHelperOutput output, ModelExplorer explorer, out List<SelectListItem> selectItems)
        {
            IStringLocalizer localizer = null;
            var resourceType = _options.AssemblyResources.GetOrDefault(explorer.ModelType.Assembly);

            if (resourceType != null)
            {
                localizer = _stringLocalizerFactory.Create(resourceType);
            }

            selectItems = explorer.Metadata.IsEnum ? explorer.ModelType.GetTypeInfo().GetMembers(BindingFlags.Public | BindingFlags.Static)
                .Select((t, i) => new SelectListItem { Value = i.ToString(), Text = GetLocalizedPropertyName(localizer, explorer.ModelType, t.Name) }).ToList() : null;

            return selectItems != null;
        }

        protected virtual string GetLocalizedPropertyName(IStringLocalizer localizer, Type enumType, string propertyName)
        {
            if (localizer == null)
            {
                return propertyName;
            }

            var localizedString = localizer[enumType.Name + "." + propertyName];

            return !localizedString.ResourceNotFound ? localizedString.Value : localizer[propertyName].Value;
        }

        protected virtual bool GetSelectItemsIfProvidedFromAttribute(TagHelperContext context, TagHelperOutput output, ModelExplorer explorer, out List<SelectListItem> selectItems)
        {
            selectItems = GetAttribute<SelectItems>(explorer)?.GetItems(explorer)?.ToList();

            return selectItems != null;
        }

        protected virtual string GetLabelAsHtmlUsingTagHelper(TagHelperContext context, TagHelperOutput output)
        {
            var labelTagHelper = new LabelTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            return RenderTagHelper(new TagHelperAttributeList(), context, labelTagHelper, _encoder, "label", TagMode.StartTagAndEndTag, true);
        }

        protected virtual string GetValidationAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
        {
            if (inputTag.Attributes.Any(a => a.Name.ToLowerInvariant() == "type" && a.Value.ToString().ToLowerInvariant() == "hidden"))
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
    }
}