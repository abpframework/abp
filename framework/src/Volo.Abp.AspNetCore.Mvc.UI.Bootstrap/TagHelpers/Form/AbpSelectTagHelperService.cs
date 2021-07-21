using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpSelectTagHelperService : AbpTagHelperService<AbpSelectTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;
        private readonly IAbpTagHelperLocalizer _tagHelperLocalizer;
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public AbpSelectTagHelperService(
            IHtmlGenerator generator,
            HtmlEncoder encoder,
            IAbpTagHelperLocalizer tagHelperLocalizer,
            IStringLocalizerFactory stringLocalizerFactory)
        {
            _generator = generator;
            _encoder = encoder;
            _tagHelperLocalizer = tagHelperLocalizer;
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();

            var innerHtml = await GetFormInputGroupAsHtmlAsync(context, output, childContent);

            var order = TagHelper.AspFor.ModelExplorer.GetDisplayOrder();

            AddGroupToFormGroupContents(context, TagHelper.AspFor.Name, SurroundInnerHtmlAndGet(context, output, innerHtml), order, out var suppress);

            if (suppress)
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

        protected virtual async Task<string> GetFormInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperContent childContent)
        {
            var selectTag = await GetSelectTagAsync(context, output, childContent);
            var selectAsHtml = selectTag.Render(_encoder);
            var label = await GetLabelAsHtmlAsync(context, output, selectTag);
            var validation = await GetValidationAsHtmlAsync(context, output, selectTag);
            var infoText = GetInfoAsHtml(context, output, selectTag);

            return label + Environment.NewLine + selectAsHtml + Environment.NewLine + infoText + Environment.NewLine + validation;
        }

        protected virtual string SurroundInnerHtmlAndGet(TagHelperContext context, TagHelperOutput output, string innerHtml)
        {
            return "<div class=\"form-group\">" + Environment.NewLine + innerHtml + Environment.NewLine + "</div>";
        }

        protected virtual async Task<TagHelperOutput> GetSelectTagAsync(TagHelperContext context, TagHelperOutput output, TagHelperContent childContent)
        {
            var selectTagHelper = new SelectTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            if (TagHelper.AutocompleteApiUrl.IsNullOrEmpty())
            {
                selectTagHelper.Items = GetSelectItems(context, output);
            }
            else if(!TagHelper.AutocompleteSelectedItemName.IsNullOrEmpty())
            {
                selectTagHelper.Items = new[]
                {
                    new SelectListItem(TagHelper.AutocompleteSelectedItemName,
                        TagHelper.AutocompleteSelectedItemValue, false)
                };
            }

            var selectTagHelperOutput = await selectTagHelper.ProcessAndGetOutputAsync(GetInputAttributes(context, output), context, "select", TagMode.StartTagAndEndTag);

            selectTagHelperOutput.Content.SetHtmlContent(childContent);
            selectTagHelperOutput.Attributes.AddClass("form-control");
            selectTagHelperOutput.Attributes.AddClass(GetSize(context, output));
            AddDisabledAttribute(selectTagHelperOutput);
            AddInfoTextId(selectTagHelperOutput);
            AddAutocompleteAttributes(selectTagHelperOutput);

            return selectTagHelperOutput;
        }

        protected virtual void AddAutocompleteAttributes(TagHelperOutput output)
        {
            if (!TagHelper.AutocompleteApiUrl.IsNullOrEmpty())
            {
                output.Attributes.AddClass("auto-complete-select");
                output.Attributes.Add("data-autocomplete-api-url", TagHelper.AutocompleteApiUrl);
                output.Attributes.Add("data-autocomplete-items-property", TagHelper.AutocompleteItemsPropertyName);
                output.Attributes.Add("data-autocomplete-display-property", TagHelper.AutocompleteDisplayPropertyName);
                output.Attributes.Add("data-autocomplete-value-property", TagHelper.AutocompleteValuePropertyName);
                output.Attributes.Add("data-autocomplete-filter-param-name", TagHelper.AutocompleteFilterParamName);
                output.Attributes.Add("data-autocomplete-selected-item-name", TagHelper.AutocompleteSelectedItemName);
                output.Attributes.Add("data-autocomplete-selected-item-value", TagHelper.AutocompleteSelectedItemValue);
            }
        }

        protected virtual void AddDisabledAttribute(TagHelperOutput inputTagHelperOutput)
        {
            var disabledAttribute = TagHelper.AspFor.ModelExplorer.GetAttribute<DisabledInput>();

            if (disabledAttribute != null && !inputTagHelperOutput.Attributes.ContainsName("disabled"))
            {
                inputTagHelperOutput.Attributes.Add("disabled", "");
            }
        }

        protected virtual List<SelectListItem> GetSelectItems(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.AspItems != null)
            {
                return TagHelper.AspItems.ToList();
            }

            if (IsEnum())
            {
                return GetSelectItemsFromEnum(context, output, TagHelper.AspFor.ModelExplorer);
            }

            var selectItemsAttribute = TagHelper.AspFor.ModelExplorer.GetAttribute<SelectItems>();
            if (selectItemsAttribute != null)
            {
                return GetSelectItemsFromAttribute(selectItemsAttribute, TagHelper.AspFor.ModelExplorer);
            }

            throw new Exception("No items provided for select attribute.");
        }

        private bool IsEnum()
        {
            var value = TagHelper.AspFor.Model;
            if (value != null && value.GetType().IsEnum)
            {
                return true;
            }

            return TagHelper.AspFor.ModelExplorer.Metadata.IsEnum;
        }

        protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput selectTag)
        {
            if (TagHelper.SuppressLabel)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(TagHelper.Label))
            {
                var label = new TagBuilder("label");
                label.Attributes.Add("for", GetIdAttributeValue(selectTag));
                label.InnerHtml.AppendHtml(TagHelper.Label);

                return label.ToHtmlString() + GetRequiredSymbol(context, output);
            }

            return await GetLabelAsHtmlUsingTagHelperAsync(context, output) + GetRequiredSymbol(context, output);
        }

        protected virtual string GetRequiredSymbol(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.DisplayRequiredSymbol)
            {
                return "";
            }

            return TagHelper.AspFor.ModelExplorer.GetAttribute<RequiredAttribute>() != null ? "<span> * </span>" : "";
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

        protected virtual string GetInfoAsHtml(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
        {
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

            var small = new TagBuilder("small");
            small.Attributes.Add("id", idAttr?.Value?.ToString() + "InfoText");
            small.AddCssClass("form-text text-muted");
            small.InnerHtml.Append(localizedText);

            return small.ToHtmlString();
        }

        protected virtual List<SelectListItem> GetSelectItemsFromEnum(TagHelperContext context, TagHelperOutput output, ModelExplorer explorer)
        {
            var selectItems = new List<SelectListItem>();
            var isNullableType = Nullable.GetUnderlyingType(explorer.ModelType) != null;
            var enumType = explorer.ModelType;

            if (isNullableType)
            {
                enumType = Nullable.GetUnderlyingType(explorer.ModelType);
                selectItems.Add(new SelectListItem());
            }

            var containerLocalizer = _tagHelperLocalizer.GetLocalizerOrNull(explorer.Container.ModelType.Assembly);

            foreach (var enumValue in enumType.GetEnumValues())
            {
                var memberName = enumType.GetEnumName(enumValue);
                var localizedMemberName = AbpInternalLocalizationHelper.LocalizeWithFallback(
                    new[]
                    {
                        containerLocalizer,
                        _stringLocalizerFactory.CreateDefaultOrNull()
                    },
                    new[]
                    {
                        $"Enum:{enumType.Name}.{memberName}",
                        $"{enumType.Name}.{memberName}",
                        memberName
                    },
                    memberName
                );

                selectItems.Add(new SelectListItem
                {
                    Value = enumValue.ToString(),
                    Text = localizedMemberName
                });
            }

            return selectItems;
        }

        protected virtual List<SelectListItem> GetSelectItemsFromAttribute(
            SelectItems selectItemsAttribute,
            ModelExplorer explorer)
        {
            var selectItems = selectItemsAttribute.GetItems(explorer)?.ToList();

            if (selectItems == null)
            {
                return new List<SelectListItem>();
            }

            return selectItems;
        }

        protected virtual async Task<string> GetLabelAsHtmlUsingTagHelperAsync(TagHelperContext context, TagHelperOutput output)
        {
            var labelTagHelper = new LabelTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            return await labelTagHelper.RenderAsync(new TagHelperAttributeList(), context, _encoder, "label", TagMode.StartTagAndEndTag);
        }

        protected virtual async Task<string> GetValidationAsHtmlAsync(TagHelperContext context, TagHelperOutput output, TagHelperOutput inputTag)
        {
            var validationMessageTagHelper = new ValidationMessageTagHelper(_generator)
            {
                For = TagHelper.AspFor,
                ViewContext = TagHelper.ViewContext
            };

            var attributeList = new TagHelperAttributeList { { "class", "text-danger" } };

            return await validationMessageTagHelper.RenderAsync(attributeList, context, _encoder, "span", TagMode.StartTagAndEndTag);
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
                    return "custom-select-sm";
                case AbpFormControlSize.Medium:
                    return "custom-select-md";
                case AbpFormControlSize.Large:
                    return "custom-select-lg";
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

            attrList.AddClass("custom-select");

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

        protected virtual string GetIdAttributeValue(TagHelperOutput inputTag)
        {
            var idAttr = inputTag.Attributes.FirstOrDefault(a => a.Name == "id");

            return idAttr != null ? idAttr.Value.ToString() : string.Empty;
        }

        protected virtual string GetIdAttributeAsString(TagHelperOutput inputTag)
        {
            var id = GetIdAttributeValue(inputTag);

            return !string.IsNullOrWhiteSpace(id) ? "for=\"" + id + "\"" : string.Empty;
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
                    Order = order,
                    PropertyName = propertyName
                });
            }
        }
    }
}
