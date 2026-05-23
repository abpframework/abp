using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

public class AbpTextTagHelperService : AbpTagHelperService<AbpTextTagHelper>
{
    protected HtmlEncoder HtmlEncoder { get; }
    private readonly IAbpTagHelperLocalizer _tagHelperLocalizer;
    protected readonly IAbpEnumLocalizer _abpEnumLocalizer;
    protected readonly IStringLocalizerFactory _stringLocalizerFactory;

    public AbpTextTagHelperService(
        HtmlEncoder htmlEncoder,
        IAbpTagHelperLocalizer tagHelperLocalizer,
        IAbpEnumLocalizer abpEnumLocalizer,
        IStringLocalizerFactory stringLocalizerFactory)
    {
        HtmlEncoder = htmlEncoder;
        _tagHelperLocalizer = tagHelperLocalizer;
        _abpEnumLocalizer = abpEnumLocalizer;
        _stringLocalizerFactory = stringLocalizerFactory;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        NormalizeTagMode(context, output);
        SetAttributes(context, output);

        var html = GetHtml(context, output);
        SetContent(context, output, html, childContent);
    }

    protected virtual void NormalizeTagMode(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "div";
    }

    protected virtual void SetAttributes(TagHelperContext context, TagHelperOutput output)
    {
        var cssClass = "mb-3";
        output.Attributes.AddIfNotContains("class", cssClass);
    }

    protected virtual void SetContent(TagHelperContext context, TagHelperOutput output, string html, TagHelperContent childContent)
    {
        var content = childContent.GetContent();

        if (!string.IsNullOrEmpty(content))
        {
            content = html + content;
        }
        else
        {
            content = html;
        }

        output.Content.SetHtmlContent(content);
    }

    protected virtual string GetHtml(TagHelperContext context, TagHelperOutput output)
    {
        var labelHtml = GetLabelHtml();
        var textHtml = GetTextHtml();
        var fieldId = GetFieldId();
        var value = TagHelper.AspFor.Model;

        var contentBuilder = new StringBuilder();
        var hidden = IsHidden() ? " d-none" : "";
        contentBuilder.AppendLine($"<div class=\"row{hidden}\">");

        if (!TagHelper.SuppressLabel)
        {
            var labelWidth = $"col-{(int)TagHelper.LabelWidth}";
            contentBuilder.AppendLine($"<div class=\"{labelWidth}\">");
            contentBuilder.AppendLine(labelHtml);
            contentBuilder.AppendLine("</div>");
        }

        contentBuilder.AppendLine($"<div class=\"col\" id=\"{fieldId}\" data-value=\"{HtmlEncoder.Encode(value?.ToString() ?? string.Empty)}\">");
        contentBuilder.AppendLine(textHtml);
        contentBuilder.AppendLine("</div>");

        contentBuilder.AppendLine("</div>");

        return contentBuilder.ToString();
    }

    protected virtual string GetFieldId()
    {
        return TagHelper.AspFor.Name
            .Replace(".", "_")
            .Replace("[", "_")
            .Replace("]", "_");
    }

    protected virtual string GetLabelHtml()
    {
        var label = TagHelper.Label ??
               TagHelper.AspFor.Metadata.DisplayName ??
               TagHelper.AspFor.Metadata.PropertyName ??
               TagHelper.AspFor.Name;

        return $"<strong>{HtmlEncoder.Encode(label)}</strong>";
    }

    protected virtual string GetTextHtml()
    {
        var value = TagHelper.AspFor.Model;

        if (value == null)
        {
            return "<span class=\"text-muted\">-</span>";
        }

        if (IsEnum())
        {
            return ToFormat(GetEnumValue());
        }

        if (IsDate())
        {
            return GetDateValue();
        }

        if (IsBoolean())
        {
            return GetBooleanValue();
        }

        if (IsFile())
        {
            return GetFileValue();
        }

        if (IsCollection())
        {
            return ToFormat(GetCollectionValue());
        }

        return ToFormat(HtmlEncoder.Encode(value.ToString()!));
    }

    protected string ToFormat(string? value)
    {
        var format = TagHelper.Format ?? TagHelper.AspFor.ModelExplorer.GetAttribute<AbpText>()?.Format ?? string.Empty;
        if (format.IsNullOrEmpty() || value.IsNullOrWhiteSpace())
        {
            return value ?? string.Empty;
        }
        return string.Format(format, value);
    }

    protected virtual string GetEnumValue()
    {
        var value = TagHelper.AspFor.Model;
        var modelType = value.GetType();
        var enumType = modelType.IsEnum ? modelType : Nullable.GetUnderlyingType(modelType);
        var containerLocalizer = _tagHelperLocalizer.GetLocalizerOrNull(TagHelper.AspFor.ModelExplorer.Container.ModelType.Assembly);
        var localizedMemberName = value == null ? "-" : _abpEnumLocalizer.GetString(enumType!, (int)value,
               new[]
               {
                containerLocalizer,
                _stringLocalizerFactory.CreateDefaultOrNull()
               }!);
        return HtmlEncoder.Encode(localizedMemberName ?? string.Empty);
    }

    protected virtual string GetDateValue()
    {
        var value = TagHelper.AspFor.Model;

        var format = TagHelper.Format ?? TagHelper.AspFor.ModelExplorer.GetAttribute<AbpText>()?.Format;

        if (value is DateTime dateTime)
        {
            return dateTime.ToString(format);
        }

        if (value is DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(format);
        }

        return value.ToString() ?? string.Empty;
    }

    protected virtual string GetBooleanValue()
    {
        var value = TagHelper.AspFor.Model;

        if (value is bool boolValue)
        {
            return boolValue
                ? "<span class=\"fa fa-check\"></span>"
                : "<span class=\"fa fa-times\"></span>";
        }

        return HtmlEncoder.Encode(value.ToString() ?? string.Empty);
    }

    protected virtual string GetFileValue()
    {
        return "<span class=\"text-info\"><i class=\"bi bi-file\"></i> File</span>";
    }

    protected virtual string GetCollectionValue()
    {
        var value = TagHelper.AspFor.Model;

        //todo Consider supporting collection render.
        if (value is System.Collections.IEnumerable enumerable)
        {
            var count = 0;
            foreach (var item in enumerable)
            {
                count++;
            }

            return count.ToString();
        }

        return HtmlEncoder.Encode(value.ToString() ?? string.Empty);
    }

    protected virtual bool IsHidden()
    {
        return TagHelper.AspFor.ModelExplorer.GetAttribute<HiddenInputAttribute>() != null;
    }

    protected virtual bool IsEnum()
    {
        return TagHelper.AspFor.ModelExplorer.Metadata.IsEnum;
    }

    protected virtual bool IsDate()
    {
        var modelType = TagHelper.AspFor.Metadata.ModelType;
        return modelType == typeof(DateTime) ||
               modelType == typeof(DateTime?) ||
               modelType == typeof(DateTimeOffset) ||
               modelType == typeof(DateTimeOffset?);
    }

    protected virtual bool IsBoolean()
    {
        var modelType = TagHelper.AspFor.Metadata.ModelType;
        return modelType == typeof(bool) || modelType == typeof(bool?);
    }

    protected virtual bool IsFile()
    {
        var modelType = TagHelper.AspFor.Metadata.ModelType;
        return typeof(IFormFile).IsAssignableFrom(modelType) ||
               typeof(IEnumerable<IFormFile>).IsAssignableFrom(modelType);
    }

    protected virtual bool IsCollection()
    {
        var modelType = TagHelper.AspFor.Metadata.ModelType;
        return typeof(System.Collections.IEnumerable).IsAssignableFrom(modelType) &&
               modelType != typeof(string);
    }
}