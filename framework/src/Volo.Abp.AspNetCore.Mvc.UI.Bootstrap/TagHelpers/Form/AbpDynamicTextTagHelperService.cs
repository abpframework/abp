using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

public class AbpDynamicTextTagHelperService : AbpTagHelperService<AbpDynamicTextTagHelper>
{
    protected HtmlEncoder HtmlEncoder { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected List<ModelExpression> Models = new();

    public AbpDynamicTextTagHelperService(
        HtmlEncoder htmlEncoder,
        IServiceProvider serviceProvider
        )
    {
        HtmlEncoder = htmlEncoder;
        ServiceProvider = serviceProvider;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        Models = GetModels(context, output);
        NormalizeTagMode(context, output);
        var childContent = await output.GetChildContentAsync();
        var html = await GetHtmlAsync(context, output);
        SetContent(context, output, html, childContent);
        SetAttributes(context, output);
    }

    protected virtual void NormalizeTagMode(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "div";
    }

    protected virtual void SetAttributes(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddIfNotContains("class", "abp-dynamic-text");
    }

    protected virtual void SetContent(TagHelperContext context, TagHelperOutput output, string html, TagHelperContent childContent)
    {
        var content = childContent.GetContent();

        if (content.Contains(AbpFormContentPlaceHolder))
        {
            content = content.Replace(AbpFormContentPlaceHolder, html);
        }
        else
        {
            content = html + content;
        }

        output.Content.SetHtmlContent(content);
    }

    protected virtual async Task<string> GetHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var contentBuilder = new StringBuilder();

        foreach (var model in Models)
        {
            var textTagHelper = GetTextTagHelper(model);
            var textHtml = await textTagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div", TagMode.StartTagAndEndTag);

            if (TagHelper.ColumnSize > ColumnSize.Undefined)
            {
                var columnSize = $"col-12 col-sm-{(int)TagHelper.ColumnSize}";
                var hidden = IsHidden(model.ModelExplorer) ? " d-none" : "";
                contentBuilder.AppendLine($"<div class=\"{columnSize}{hidden}\">{textHtml}</div>");
            }
            else
            {
                contentBuilder.AppendLine(textHtml);
            }
        }

        if (TagHelper.ColumnSize > ColumnSize.Undefined)
        {
            contentBuilder.Insert(0, "<div class=\"row\">");
            contentBuilder.AppendLine("</div>");
        }

        return contentBuilder.ToString();
    }

    protected virtual AbpTextTagHelper GetTextTagHelper(ModelExpression model)
    {
        var textTagHelper = ServiceProvider.GetRequiredService<AbpTextTagHelper>();
        textTagHelper.AspFor = model;
        textTagHelper.ViewContext = TagHelper.ViewContext;

        var textAttribute = model.ModelExplorer.GetAttribute<AbpText>();
        if (textAttribute == null)
        {
            textTagHelper.LabelWidth = TagHelper.LabelWidth;
            return textTagHelper;
        }

        textTagHelper.Format = textAttribute.Format;
        textTagHelper.LabelWidth = textAttribute.LabelWidth ?? TagHelper.LabelWidth;
        textTagHelper.SuppressLabel = textAttribute.SuppressLabel;

        return textTagHelper;
    }

    protected bool IsHidden(ModelExplorer model)
    {
        return model.GetAttribute<HiddenInputAttribute>() != null;
    }

    protected virtual List<ModelExpression> GetModels(TagHelperContext context, TagHelperOutput output)
    {
        return TagHelper.Model.ModelExplorer.Properties.Aggregate(new List<ModelExpression>(), ExploreModelsRecursively);
    }

    protected virtual List<ModelExpression> ExploreModelsRecursively(List<ModelExpression> list, ModelExplorer model)
    {
        if (model.GetAttribute<DynamicFormIgnore>() != null)
        {
            return list;
        }

        if (IsCsharpClassOrPrimitive(model.ModelType) || IsListOfCsharpClassOrPrimitive(model.ModelType))
        {
            list.Add(ModelExplorerToModelExpressionConverter(model));
            return list;
        }

        if (IsListOfSelectItem(model.ModelType) || IsFile(model.ModelType))
        {
            list.Add(ModelExplorerToModelExpressionConverter(model));
            return list;
        }

        return model.Properties.Aggregate(list, ExploreModelsRecursively);
    }

    protected virtual ModelExpression ModelExplorerToModelExpressionConverter(ModelExplorer explorer)
    {
        var temp = explorer;
        var propertyName = explorer.Metadata.PropertyName;

        while (temp?.Container?.Metadata?.PropertyName != null)
        {
            temp = temp.Container;
            propertyName = temp.Metadata.PropertyName + "." + propertyName;
        }

        return new ModelExpression(propertyName ?? string.Empty, explorer);
    }

    protected virtual bool IsListOfCsharpClassOrPrimitive(Type type)
    {
        var genericType = type.GenericTypeArguments.FirstOrDefault();

        if (genericType == null || !IsCsharpClassOrPrimitive(genericType))
        {
            return false;
        }

        return type.ToString().StartsWith("System.Collections.Generic.IEnumerable`") ||
               type.ToString().StartsWith("System.Collections.Generic.List`");
    }

    protected virtual bool IsCsharpClassOrPrimitive(Type? type)
    {
        if (type == null) return false;

        return type.IsPrimitive ||
               type.IsValueType ||
               type == typeof(string) ||
               type == typeof(Guid) ||
               type == typeof(DateTime) ||
               type == typeof(ValueType) ||
               type == typeof(TimeSpan) ||
               type == typeof(DateTimeOffset) ||
               type.IsEnum;
    }

    protected virtual bool IsListOfSelectItem(Type type)
    {
        return type == typeof(List<SelectListItem>) ||
               type == typeof(IEnumerable<SelectListItem>);
    }

    protected virtual bool IsFile(Type type)
    {
        return typeof(IFormFile).IsAssignableFrom(type) ||
               typeof(IEnumerable<IFormFile>).IsAssignableFrom(type);
    }
}
