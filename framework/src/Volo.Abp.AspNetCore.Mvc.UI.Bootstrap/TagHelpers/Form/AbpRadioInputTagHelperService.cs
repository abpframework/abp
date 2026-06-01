using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

public class AbpRadioInputTagHelperService : AbpTagHelperService<AbpRadioInputTagHelper>
{
    private readonly IAbpTagHelperLocalizer _tagHelperLocalizer;
    private readonly IHtmlGenerator _generator;
    private readonly HtmlEncoder _encoder;
    private readonly IStringLocalizerFactory _stringLocalizerFactory;
    private readonly IAbpEnumLocalizer _abpEnumLocalizer;

    public AbpRadioInputTagHelperService(
        IAbpTagHelperLocalizer tagHelperLocalizer,
        IHtmlGenerator generator,
        HtmlEncoder encoder,
        IStringLocalizerFactory stringLocalizerFactory,
        IAbpEnumLocalizer abpEnumLocalizer)
    {
        _tagHelperLocalizer = tagHelperLocalizer;
        _generator = generator;
        _encoder = encoder;
        _stringLocalizerFactory = stringLocalizerFactory;
        _abpEnumLocalizer = abpEnumLocalizer;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var selectItems = GetSelectItems(context, output);
        SetSelectedValue(context, output, selectItems);

        var order = TagHelper.AspFor.ModelExplorer.GetDisplayOrder();

        var html = await GetRadioInputGroupAsHtmlAsync(context, output, selectItems);

        AddGroupToFormGroupContents(context, TagHelper.AspFor.Name, html, order, out var suppress);

        if (suppress)
        {
            output.SuppressOutput();
        }
        else
        {
            output.TagName = "div";
            output.Attributes.Clear();
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(html);
        }
    }

    protected virtual async Task<string> GetRadioInputGroupAsHtmlAsync(TagHelperContext context, TagHelperOutput output, List<SelectListItem> selectItems)
    {
        var radioGroupHtml = GetHtml(context, output, selectItems);
        var label = await GetLabelAsHtmlAsync(context, output);
        var infoText = GetInfoAsHtml(context, output);

        var tagBuilder = new TagBuilder("div");
        tagBuilder.AddCssClass("mb-3");
        tagBuilder.InnerHtml.AppendHtml(label);
        tagBuilder.InnerHtml.AppendHtml(radioGroupHtml);
        tagBuilder.InnerHtml.AppendHtml(infoText);

        return tagBuilder.ToHtmlString();
    }

    protected virtual string GetHtml(TagHelperContext context, TagHelperOutput output, List<SelectListItem> selectItems)
    {
        var html = new StringBuilder("");

        foreach (var selectItem in selectItems)
        {
            var inlineClass = (TagHelper.Inline ?? false) ? " form-check-inline" : "";
            var id = TagHelper.AspFor.Name + "Radio" + selectItem.Value;
            var name = TagHelper.AspFor.Name;

            var input = new TagBuilder("input");
            input.AddCssClass("form-check-input");
            input.Attributes.Add("type", "radio");
            input.Attributes.Add("id", id);
            input.Attributes.Add("name", name);
            input.Attributes.Add("value", selectItem.Value);

            if (selectItem.Selected)
            {
                input.Attributes.Add("checked", "checked");
            }

            if (TagHelper.Disabled ?? false)
            {
                input.Attributes.Add("disabled", "disabled");
            }

            var label = new TagBuilder("label");
            label.AddCssClass("form-check-label");
            label.Attributes.Add("for", id);
            label.InnerHtml.Append(selectItem.Text);

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("form-check mb-1" + inlineClass);
            wrapper.InnerHtml.AppendHtml(input);
            wrapper.InnerHtml.AppendHtml(label);

            html.AppendLine(wrapper.ToHtmlString());
        }

        var div = new TagBuilder("div");
        div.AddCssClass("mb-1");
        div.InnerHtml.AppendHtml(html.ToString());

        return div.ToHtmlString();
    }

    protected virtual async Task<string> GetLabelAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.SuppressLabel)
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(TagHelper.Label))
        {
            return await GetLabelAsHtmlUsingTagHelperAsync(context, output);
        }

        var label = new TagBuilder("label");
        label.AddCssClass("form-label");
        label.InnerHtml.AppendHtml(TagHelper.Label);
        label.InnerHtml.AppendHtml(GetRequiredSymbol(context, output));

        return label.ToHtmlString();
    }

    protected virtual async Task<string> GetLabelAsHtmlUsingTagHelperAsync(TagHelperContext context, TagHelperOutput output)
    {
        var labelTagHelper = new LabelTagHelper(_generator)
        {
            For = TagHelper.AspFor,
            ViewContext = TagHelper.ViewContext,
        };

        var innerOutput = await labelTagHelper.ProcessAndGetOutputAsync(
            new TagHelperAttributeList { { "class", "form-label" } },
            context,
            "label",
            TagMode.StartTagAndEndTag);

        innerOutput.Content.AppendHtml(GetRequiredSymbol(context, output));

        return innerOutput.Render(_encoder);
    }

    protected virtual string GetRequiredSymbol(TagHelperContext context, TagHelperOutput output)
    {
        var isHaveRequiredAttribute = context.AllAttributes.Any(a => a.Name == "required");

        return TagHelper.AspFor.ModelExplorer.GetAttribute<RequiredAttribute>() != null || isHaveRequiredAttribute
            ? "<span> * </span>"
            : "";
    }

    protected virtual string GetInfoAsHtml(TagHelperContext context, TagHelperOutput output)
    {
        var text = string.Empty;
        var infoAttribute = TagHelper.AspFor.ModelExplorer.GetAttribute<InputInfoText>();

        if (!string.IsNullOrEmpty(TagHelper.InfoText))
        {
            text = TagHelper.InfoText!;
        }
        else if (infoAttribute != null)
        {
            text = _tagHelperLocalizer.GetLocalizedText(infoAttribute.Text, TagHelper.AspFor.ModelExplorer);
        }
        else
        {
            return "";
        }

        var small = new TagBuilder("small");
        small.Attributes.Add("id", TagHelper.AspFor.Name.Replace('.', '_') + "InfoText");
        small.AddCssClass("form-text");
        small.InnerHtml.Append(text);

        return small.ToHtmlString();
    }

    protected virtual List<SelectListItem> GetSelectItems(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.AspItems != null)
        {
            return TagHelper.AspItems.ToList();
        }

        if (TagHelper.AspFor.ModelExplorer.Metadata.IsEnum)
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

    protected virtual List<SelectListItem> GetSelectItemsFromEnum(TagHelperContext context, TagHelperOutput output, ModelExplorer explorer)
    {
        var selectItems = new List<SelectListItem>();
        var isNullableType = Nullable.GetUnderlyingType(explorer.ModelType) != null;
        var enumType = explorer.ModelType;

        if (isNullableType)
        {
            enumType = Nullable.GetUnderlyingType(explorer.ModelType)!;
            selectItems.Add(new SelectListItem());
        }

        var containerLocalizer = _tagHelperLocalizer.GetLocalizerOrNull(explorer.Container.ModelType.Assembly);

        foreach (var enumValue in enumType.GetEnumValuesAsUnderlyingType())
        {
            var localizedMemberName = _abpEnumLocalizer.GetString(enumType, enumValue,
                new[]
                {
                    containerLocalizer,
                    _stringLocalizerFactory.CreateDefaultOrNull()
                }!);
            selectItems.Add(new SelectListItem
            {
                Value = enumValue.ToString(),
                Text = localizedMemberName
            });
        }

        return selectItems;
    }

    protected virtual string GetLocalizedPropertyName(IStringLocalizer? localizer, Type enumType, string propertyName)
    {
        if (localizer == null)
        {
            return propertyName;
        }

        var localizedString = localizer[enumType.Name + "." + propertyName];

        return !localizedString.ResourceNotFound ? localizedString.Value : localizer[propertyName].Value;
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

    protected virtual void SetSelectedValue(TagHelperContext context, TagHelperOutput output, List<SelectListItem> selectItems)
    {
        var selectedValue = GetSelectedValue(context, output);

        if (!selectItems.Any(si => si.Selected))
        {
            var itemToBeSelected = selectItems.FirstOrDefault(si => si.Value == selectedValue);

            if (itemToBeSelected != null)
            {
                itemToBeSelected.Selected = true;
            }
        }
    }

    protected virtual string? GetSelectedValue(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.AspFor.ModelExplorer.Metadata.IsEnum)
        {
            var baseType = TagHelper.AspFor.ModelExplorer.Model?.GetType().GetEnumUnderlyingType();

            if (baseType == null)
            {
                return null;
            }

            var valueAsString = Convert.ChangeType(TagHelper.AspFor.ModelExplorer.Model, baseType);
            return valueAsString != null ? valueAsString.ToString() : "";
        }

        return TagHelper.AspFor.ModelExplorer.Model?.ToString();
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
