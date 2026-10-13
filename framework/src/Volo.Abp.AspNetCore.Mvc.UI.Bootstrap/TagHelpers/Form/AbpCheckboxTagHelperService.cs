using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

public class AbpCheckboxTagHelperService : AbpTagHelperService<AbpCheckboxTagHelper>
{
    private readonly IAbpTagHelperLocalizer _tagHelperLocalizer;
    private readonly IStringLocalizerFactory _stringLocalizerFactory;
    private readonly IAbpEnumLocalizer _abpEnumLocalizer;
    private readonly IHtmlGenerator _generator;
    private readonly HtmlEncoder _encoder;

    public AbpCheckboxTagHelperService(
        IAbpTagHelperLocalizer tagHelperLocalizer,
        IStringLocalizerFactory stringLocalizerFactory,
        IAbpEnumLocalizer abpEnumLocalizer,
        IHtmlGenerator generator,
        HtmlEncoder encoder)
    {
        _tagHelperLocalizer = tagHelperLocalizer;
        _stringLocalizerFactory = stringLocalizerFactory;
        _abpEnumLocalizer = abpEnumLocalizer;
        _generator = generator;
        _encoder = encoder;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var selectItems = GetSelectItems(context, output);

        var order = TagHelper.AspFor.ModelExplorer.GetDisplayOrder();
        var html = await GetHtmlAsync(context, output, selectItems);

        AddGroupToFormGroupContents(context, TagHelper.AspFor.Name, html, order, out var suppress);

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", BuildWrapperCssClass());
        output.Content.SetHtmlContent(html);
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

    protected virtual string BuildWrapperCssClass()
    {
        return string.IsNullOrWhiteSpace(TagHelper.CssClass)
            ? "abp-checkbox-wrapper"
            : $"abp-checkbox-wrapper {TagHelper.CssClass}";
    }

    protected virtual List<SelectListItem> GetSelectItems(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.AspItems != null)
        {
            return TagHelper.AspItems.ToList();
        }

        var selectItemsAttribute = TagHelper.AspFor.ModelExplorer.GetAttribute<SelectItems>();
        if (selectItemsAttribute != null)
        {
            return GetSelectItemsFromAttribute(selectItemsAttribute, TagHelper.AspFor.ModelExplorer);
        }

        var enumType = ResolveEnumType(context, output, TagHelper.AspFor.ModelExplorer);
        if (enumType != null)
        {
            return GetSelectItemsFromEnum(enumType);
        }

        throw new Exception("No items provided for select attribute.");
    }

    protected virtual Type? ResolveEnumType(TagHelperContext context, TagHelperOutput output, ModelExplorer explorer)
    {
        var modelType = explorer.ModelType;

        var underlying = Nullable.GetUnderlyingType(modelType);
        if (underlying != null && underlying.IsEnum)
        {
            return underlying;
        }

        if (modelType.IsEnum)
        {
            return modelType;
        }

        if (modelType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(modelType))
        {
            if (modelType.IsArray)
            {
                var elementType = modelType.GetElementType();
                var elemUnderlying = elementType != null ? Nullable.GetUnderlyingType(elementType) : null;
                if (elemUnderlying?.IsEnum == true) return elemUnderlying;
                if (elementType?.IsEnum == true) return elementType;
            }

            var enumerableInterface = modelType
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (enumerableInterface != null)
            {
                var elementType = enumerableInterface.GetGenericArguments()[0];
                var elemUnderlying = Nullable.GetUnderlyingType(elementType);
                if (elemUnderlying?.IsEnum == true) return elemUnderlying;
                if (elementType.IsEnum) return elementType;
            }
        }

        if (explorer.Model is IEnumerable enumerableModel && explorer.Model is not string)
        {
            foreach (var item in enumerableModel)
            {
                if (item == null) continue;
                var runtimeType = item.GetType();
                var runtimeUnderlying = Nullable.GetUnderlyingType(runtimeType);
                if (runtimeUnderlying?.IsEnum == true) return runtimeUnderlying;
                if (runtimeType.IsEnum) return runtimeType;
                break;
            }
        }

        return null;
    }

    protected virtual List<SelectListItem> GetSelectItemsFromAttribute(SelectItems selectItemsAttribute, ModelExplorer explorer)
    {
        var selectItems = selectItemsAttribute.GetItems(explorer)?.ToList();

        if (selectItems == null)
        {
            return new List<SelectListItem>();
        }

        return selectItems;
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

    protected virtual List<SelectListItem> GetSelectItemsFromEnum(Type enumType)
    {
        var selectItems = new List<SelectListItem>();

        var containerLocalizer = _tagHelperLocalizer.GetLocalizerOrNull(
            TagHelper.AspFor.ModelExplorer.Container.ModelType.Assembly);

        foreach (var enumValue in enumType.GetEnumValuesAsUnderlyingType())
        {
            var localizedName = _abpEnumLocalizer.GetString(
                enumType,
                enumValue,
                new[]
                {
                    containerLocalizer,
                    _stringLocalizerFactory.CreateDefaultOrNull()
                }!);

            selectItems.Add(new SelectListItem
            {
                Value = enumValue.ToString(),
                Text = localizedName
            });
        }

        return selectItems;
    }

    protected virtual HashSet<string?> GetSelectedValues()
    {
        var model = TagHelper.AspFor.Model;

        if (model == null)
        {
            return new HashSet<string?>();
        }

        if (model is string singleValue)
        {
            return new HashSet<string?> { singleValue };
        }

        if (model is IEnumerable enumerable)
        {
            return enumerable.Cast<object?>()
                             .Select(x => x?.ToString())
                             .ToHashSet();
        }

        return new HashSet<string?> { model.ToString() };
    }

    protected virtual string GetColumnCssClass()
    {
        var columnCss = "";

        switch (TagHelper.ColumnSize)
        {
            case ColumnSize._1:
            case ColumnSize._2:
            case ColumnSize._3:
            case ColumnSize._4:
            case ColumnSize._5:
            case ColumnSize._6:
            case ColumnSize._7:
            case ColumnSize._8:
            case ColumnSize._9:
            case ColumnSize._10:
            case ColumnSize._11:
            case ColumnSize._12:
                columnCss = $"col-{(int)TagHelper.ColumnSize}"; break;
            case ColumnSize.Auto:
            case ColumnSize._:
                columnCss = "col-auto"; break;
            case ColumnSize.Undefined:
                columnCss = ""; break;
        }

        return columnCss;
    }

    protected virtual async Task<string> GetHtmlAsync(TagHelperContext context, TagHelperOutput output, List<SelectListItem> selectItems)
    {
        var selectedValues = GetSelectedValues();

        var checkBoxWrapper = new TagBuilder("div");
        checkBoxWrapper.AddCssClass("row");

        foreach (var item in selectItems)
        {
            checkBoxWrapper.InnerHtml.AppendHtml(BuildCheckboxItem(item, selectedValues, GetColumnCssClass()));
        }

        var label = await GetLabelAsHtmlAsync(context, output);
        var infoText = GetInfoAsHtml(context, output);

        var tagBuilder = new TagBuilder("div");
        tagBuilder.AddCssClass("mb-3");
        tagBuilder.InnerHtml.AppendHtml(label);
        tagBuilder.InnerHtml.AppendHtml(checkBoxWrapper.ToHtmlString());
        tagBuilder.InnerHtml.AppendHtml(infoText);

        return tagBuilder.ToHtmlString();
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

    protected virtual IHtmlContent BuildCheckboxItem(
        SelectListItem item,
        HashSet<string?> selectedValues,
        string columnCssClass)
    {
        var value = item.Value ?? string.Empty;
        var name = TagHelper.AspFor.Name;
        var id = $"{name}_{value}";

        var col = new TagBuilder("div");
        col.AddCssClass(columnCssClass);
        col.AddCssClass("mb-2");

        var formCheck = new TagBuilder("div");
        formCheck.AddCssClass("form-check");

        var input = new TagBuilder("input");
        input.AddCssClass("form-check-input");
        input.Attributes.Add("type", "checkbox");
        input.Attributes.Add("name", name);
        input.Attributes.Add("id", id);
        input.Attributes.Add("value", value);

        if (selectedValues.Contains(item.Value))
        {
            input.Attributes.Add("checked", "checked");
        }

        if (TagHelper.Disabled || item.Disabled)
        {
            input.Attributes.Add("disabled", "disabled");
        }

        if (TagHelper.ReadOnly)
        {
            input.Attributes.Add("readonly", "readonly");
        }

        var label = new TagBuilder("label");
        label.AddCssClass("form-check-label");
        if (!string.IsNullOrEmpty(TagHelper.LabelClass))
        {
            label.AddCssClass(TagHelper.LabelClass!);
        }
        label.Attributes.Add("for", id);
        label.InnerHtml.Append(item.Text ?? string.Empty);

        formCheck.InnerHtml.AppendHtml(input);
        formCheck.InnerHtml.AppendHtml(label);
        col.InnerHtml.AppendHtml(formCheck);

        return col;
    }
}