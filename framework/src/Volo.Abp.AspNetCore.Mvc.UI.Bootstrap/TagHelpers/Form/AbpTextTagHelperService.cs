using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

public class AbpTextTagHelperService : AbpTagHelperService<AbpTextTagHelper>
{
    protected HtmlEncoder HtmlEncoder { get; }
    private readonly IAbpTagHelperLocalizer _tagHelperLocalizer;
    protected readonly IAbpEnumLocalizer _abpEnumLocalizer;
    protected readonly IStringLocalizerFactory _stringLocalizerFactory;
    private readonly ColumnSize DefaultLabelWidth = ColumnSize._4;

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
        var label = GetLabel();
        var text = GetText();
        var fieldId = GetFieldId();
        var value = TagHelper.AspFor.Model;

        var contentBuilder = new StringBuilder();
        var hidden = IsHidden() ? " d-none" : "";
        contentBuilder.AppendLine($"<div class=\"row{hidden}\">");

        if (!TagHelper.SuppressLabel)
        {
            var width = TagHelper.LabelWidth ??
                TagHelper.AspFor.ModelExplorer.GetAttribute<AbpText>()?.LabelWidth ??
                DefaultLabelWidth;
            var labelWidth = $"col-{(int)width}";
            contentBuilder.AppendLine($"<div class=\"{labelWidth}\">");
            contentBuilder.AppendLine(label);
            contentBuilder.AppendLine("</div>");
        }

        contentBuilder.AppendLine($"<div class=\"col\" id=\"{fieldId}\" data-value=\"{HtmlEncoder.Encode(value?.ToString() ?? string.Empty)}\">");
        contentBuilder.AppendLine(text);
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

    protected virtual string GetLabel()
    {
        var label = TagHelper.Label ??
               TagHelper.AspFor.Metadata.DisplayName ??
               TagHelper.AspFor.Metadata.PropertyName ??
               TagHelper.AspFor.Name;

        return $"<strong>{HtmlEncoder.Encode(label)}</strong>";
    }

    protected virtual string GetText()
    {
        if (IsEnum())
        {
            return EnumText();
        }
        if (IsDate())
        {
            return DateText();
        }
        if (IsBoolean())
        {
            return BooleanText();
        }
        if (IsFile())
        {
            return FileText();
        }
        if (IsCollection())
        {
            return CollectionText();
        }

        return DefaultText();
    }

    protected virtual string DefaultText()
    {
        var value = TagHelper.AspFor.Model;
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return "<span class=\"text-muted\">-</span>";
        }
        return StringFormat(HtmlEncoder.Encode(value.ToString()!));
    }

    protected virtual string StringFormat(string? value)
    {
        var format = GetFormatString();
        if (format.IsNullOrEmpty() || value.IsNullOrWhiteSpace())
        {
            return value ?? string.Empty;
        }
        return string.Format(format, value);
    }

    private string GetFormatString()
    {
        return TagHelper.Format ?? TagHelper.AspFor.ModelExplorer.GetAttribute<AbpText>()?.Format ?? string.Empty;
    }

    protected virtual string EnumText()
    {
        var value = TagHelper.AspFor.Model;
        if (value == null)
        {
            return DefaultText();
        }

        var modelType = value.GetType();
        var enumType = modelType.IsEnum ? modelType : Nullable.GetUnderlyingType(modelType);
        var containerLocalizer = _tagHelperLocalizer.GetLocalizerOrNull(TagHelper.AspFor.ModelExplorer.Container.ModelType.Assembly);
        var localizedMemberName = value == null ? "-" : _abpEnumLocalizer.GetString(enumType!, (int)value,
               new[]
               {
                containerLocalizer,
                _stringLocalizerFactory.CreateDefaultOrNull()
               }!);
        return StringFormat(HtmlEncoder.Encode(localizedMemberName ?? string.Empty));
    }

    protected virtual string DateText()
    {
        var value = TagHelper.AspFor.Model;
        if (value == null)
        {
            return DefaultText();
        }

        var format = GetFormatString();
        if (value is DateTime dateTime)
        {
            return dateTime.ToString(format);
        }

        if (value is DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString(format);
        }

        return StringFormat(value.ToString());
    }

    protected virtual string BooleanText()
    {
        var value = TagHelper.AspFor.Model;
        if (value == null)
        {
            return DefaultText();
        }

        if (value is bool boolValue)
        {
            return boolValue
                ? "<span class=\"fa fa-check\"></span>"
                : "<span class=\"fa fa-times\"></span>";
        }

        return StringFormat(value.ToString());
    }

    protected virtual string FileText()
    {
        return "<span class=\"text-info\"><i class=\"bi bi-file\"></i></span>";
    }

    protected virtual string CollectionText()
    {
        var value = TagHelper.AspFor.Model;
        if (value == null)
        {
            return DefaultText();
        }

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

        return StringFormat(value.ToString());
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