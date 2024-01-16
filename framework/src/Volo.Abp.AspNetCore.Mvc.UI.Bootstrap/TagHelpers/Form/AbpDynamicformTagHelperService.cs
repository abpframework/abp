using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form.DatePicker;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

public class AbpDynamicFormTagHelperService : AbpTagHelperService<AbpDynamicFormTagHelper>
{
    protected HtmlEncoder HtmlEncoder { get; }
    protected IHtmlGenerator HtmlGenerator { get; }
    protected IServiceProvider _serviceProvider { get; }
    protected IStringLocalizer<AbpUiResource> Localizer { get; }
    protected List<ModelExpression> Models = new();

    public AbpDynamicFormTagHelperService(
        HtmlEncoder htmlEncoder,
        IHtmlGenerator htmlGenerator,
        IServiceProvider serviceProvider,
        IStringLocalizer<AbpUiResource> localizer)
    {
        HtmlEncoder = htmlEncoder;
        HtmlGenerator = htmlGenerator;
        _serviceProvider = serviceProvider;
        Localizer = localizer;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        Models = GetModels(context, output);
        var list = InitilizeFormGroupContentsContext(context, output);

        NormalizeTagMode(context, output);

        var childContent = await output.GetChildContentAsync();

        await ConvertToMvcForm(context, output);

        await ProcessFieldsAsync(context, output);

        RemoveFormGroupItemsNotInModel(context, output, list);

        SetContent(context, output, list, childContent);

        SetFormAttributes(context, output);

        await SetSubmitButton(context, output);
    }

    protected virtual async Task ConvertToMvcForm(TagHelperContext context, TagHelperOutput output)
    {
        var formTagHelper = new FormTagHelper(HtmlGenerator)
        {
            Action = TagHelper.Action,
            Controller = TagHelper.Controller,
            Area = TagHelper.Area,
            Page = TagHelper.Page,
            PageHandler = TagHelper.PageHandler,
            Antiforgery = true,
            Fragment = TagHelper.Fragment,
            Route = TagHelper.Route,
            Method = TagHelper.Method,
            RouteValues = TagHelper.RouteValues,
            ViewContext = TagHelper.ViewContext
        };

        var formTagOutput = await formTagHelper.ProcessAndGetOutputAsync(output.Attributes, context, "form", TagMode.StartTagAndEndTag);

        await formTagOutput.GetChildContentAsync();

        output.PostContent.AppendHtml(formTagOutput.PostContent);
        output.PreContent.AppendHtml(formTagOutput.PreContent);
    }

    protected virtual void NormalizeTagMode(TagHelperContext context, TagHelperOutput output)
    {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.TagName = "form";
    }

    protected virtual void SetFormAttributes(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.AddIfNotContains("method", "post");
    }

    protected virtual void SetContent(TagHelperContext context, TagHelperOutput output, List<FormGroupItem> items, TagHelperContent childContent)
    {
        var contentBuilder = new StringBuilder("");

        foreach (var item in items.OrderBy(o => o.Order))
        {
            contentBuilder.AppendLine(SetColumn(item.HtmlContent));
        }

        if (TagHelper.ColumnSize != ColumnSize.Undefined && TagHelper.ColumnSize != ColumnSize._)
        {
            contentBuilder.Insert(0, "<div class=\"row\">");
            contentBuilder.AppendLine("</div>");
        }

        var content = childContent.GetContent();
        if (content.Contains(AbpFormContentPlaceHolder))
        {
            content = content.Replace(AbpFormContentPlaceHolder, contentBuilder.ToString());
        }
        else
        {
            content = contentBuilder + content;
        }

        output.Content.SetHtmlContent(content);
    }

    protected virtual string SetColumn(string htmlContent)
    {
        if (TagHelper.ColumnSize == ColumnSize.Undefined || TagHelper.ColumnSize == ColumnSize._)
        {
            return htmlContent;
        }

        var col_class = $"col-12 col-sm-" + ((int)TagHelper.ColumnSize);

        return $"<div class=\"{col_class}\">{htmlContent}</div>";
    }

    protected virtual async Task SetSubmitButton(TagHelperContext context, TagHelperOutput output)
    {
        if (!TagHelper.SubmitButton ?? true)
        {
            return;
        }

        var buttonHtml = await ProcessSubmitButtonAndGetContentAsync(context, output);

        output.PostContent.AppendHtml(buttonHtml);
    }

    protected virtual List<FormGroupItem> InitilizeFormGroupContentsContext(TagHelperContext context, TagHelperOutput output)
    {
        var items = new List<FormGroupItem>();
        context.Items[FormGroupContents] = items;
        return items;
    }

    protected virtual async Task ProcessFieldsAsync(TagHelperContext context, TagHelperOutput output)
    {
        foreach (var model in Models)
        {
            if (IsSelectGroup(context, model))
            {
                await ProcessSelectGroupAsync(context, output, model);
            }
            else if (IsDateGroup(context, model))
            {
                await ProcessDateGroupAsync(context, output, model);
            }
            else
            {
                await ProcessInputGroupAsync(context, output, model);
            }
        }
    }

    protected virtual async Task ProcessDateGroupAsync(TagHelperContext context, TagHelperOutput output, ModelExpression model)
    {
        var abpDateTagHelper = GetDateGroupTagHelper(context, output, model);

        await abpDateTagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div", TagMode.StartTagAndEndTag);
    }

    protected virtual AbpTagHelper GetDateGroupTagHelper(TagHelperContext context, TagHelperOutput output, ModelExpression model)
    {
        return IsDateRangeGroup(model.ModelExplorer)
            ? GetAbpDateRangeInputTagHelper(context, output, model)
            : GetAbpDateInputTagHelper(model);
    }

    protected virtual AbpTagHelper GetAbpDateInputTagHelper(ModelExpression model)
    {
        var abpDateInputTagHelper = _serviceProvider.GetRequiredService<AbpDatePickerTagHelper>();
        abpDateInputTagHelper.AspFor = model;
        abpDateInputTagHelper.ViewContext = TagHelper.ViewContext;
        abpDateInputTagHelper.DisplayRequiredSymbol = TagHelper.RequiredSymbols ?? true;
        return abpDateInputTagHelper;
    }

    protected virtual AbpTagHelper GetAbpDateRangeInputTagHelper(TagHelperContext context, TagHelperOutput output, ModelExpression model)
    {
        var modelAttribute = model.ModelExplorer.GetAttribute<DateRangePickerAttribute>()!;

        var pickerId = modelAttribute.PickerId;

        var abpDateRangeInputTagHelper = _serviceProvider.GetRequiredService<AbpDateRangePickerTagHelper>();
        abpDateRangeInputTagHelper.PickerId = pickerId;
        abpDateRangeInputTagHelper.ViewContext = TagHelper.ViewContext;

        if (modelAttribute.IsStart)
        {
            abpDateRangeInputTagHelper.AspForStart = model;

            var otherModelExists = TryToGetOtherDateModel(model, pickerId, out var otherModel);
            if (otherModelExists && otherModel!.GetAttribute<DateRangePickerAttribute>()!.IsEnd)
            {
                abpDateRangeInputTagHelper.AspForEnd = ModelExplorerToModelExpressionConverter(otherModel!);
            }
        }

        return abpDateRangeInputTagHelper;
    }

    protected virtual bool TryToGetOtherDateModel(ModelExpression model, string pickerId, out ModelExplorer? otherModel)
    {
        otherModel = Models.Select(x => x.ModelExplorer).SingleOrDefault(x => x != model.ModelExplorer && x.GetAttribute<DateRangePickerAttribute>()?.PickerId == pickerId);
        return otherModel != null;
    }

    protected virtual bool IsDateRangeGroup(ModelExplorer modelModelExplorer)
    {
        return modelModelExplorer.GetAttribute<DateRangePickerAttribute>() != null;
    }

    protected virtual void RemoveFormGroupItemsNotInModel(TagHelperContext context, TagHelperOutput output, List<FormGroupItem> items)
    {
        var models = GetModels(context, output);

        items.RemoveAll(x => models.All(m => !m.Name.Equals(x.PropertyName, StringComparison.InvariantCultureIgnoreCase)));
    }

    protected virtual async Task ProcessSelectGroupAsync(TagHelperContext context, TagHelperOutput output, ModelExpression model)
    {
        var abpSelectTagHelper = GetSelectGroupTagHelper(context, output, model);

        await abpSelectTagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div", TagMode.StartTagAndEndTag);
    }

    protected virtual AbpTagHelper GetSelectGroupTagHelper(TagHelperContext context, TagHelperOutput output, ModelExpression model)
    {
        return IsRadioGroup(model.ModelExplorer) ?
            GetAbpRadioInputTagHelper(model) :
            GetSelectTagHelper(model);
    }

    protected virtual AbpTagHelper GetSelectTagHelper(ModelExpression model)
    {
        var abpSelectTagHelper = _serviceProvider.GetRequiredService<AbpSelectTagHelper>();
        abpSelectTagHelper.AspFor = model;
        abpSelectTagHelper.AspItems = null;
        abpSelectTagHelper.ViewContext = TagHelper.ViewContext;
        return abpSelectTagHelper;
    }

    protected virtual AbpTagHelper GetAbpRadioInputTagHelper(ModelExpression model)
    {
        var radioButtonAttribute = model.ModelExplorer.GetAttribute<AbpRadioButton>()!;
        var abpRadioInputTagHelper = _serviceProvider.GetRequiredService<AbpRadioInputTagHelper>();
        abpRadioInputTagHelper.AspFor = model;
        abpRadioInputTagHelper.AspItems = null;
        abpRadioInputTagHelper.Inline = radioButtonAttribute.Inline;
        abpRadioInputTagHelper.Disabled = radioButtonAttribute.Disabled;
        abpRadioInputTagHelper.ViewContext = TagHelper.ViewContext;
        return abpRadioInputTagHelper;
    }

    protected virtual async Task<string> ProcessSubmitButtonAndGetContentAsync(TagHelperContext context, TagHelperOutput output)
    {
        var abpButtonTagHelper = _serviceProvider.GetRequiredService<AbpButtonTagHelper>();
        var attributes = new TagHelperAttributeList { new TagHelperAttribute("type", "submit") };
        abpButtonTagHelper.Text = Localizer["Submit"];
        abpButtonTagHelper.ButtonType = AbpButtonType.Primary;

        return await abpButtonTagHelper.RenderAsync(attributes, context, HtmlEncoder, "button", TagMode.StartTagAndEndTag);
    }

    protected virtual async Task ProcessInputGroupAsync(TagHelperContext context, TagHelperOutput output, ModelExpression model)
    {
        var abpInputTagHelper = _serviceProvider.GetRequiredService<AbpInputTagHelper>();
        abpInputTagHelper.AspFor = model;
        abpInputTagHelper.ViewContext = TagHelper.ViewContext;
        abpInputTagHelper.DisplayRequiredSymbol = TagHelper.RequiredSymbols ?? true;

        await abpInputTagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div", TagMode.StartTagAndEndTag);
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

        if (IsListOfSelectItem(model.ModelType))
        {
            return list;
        }
        
        if (IsFile(model.ModelType))
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

        return new ModelExpression(propertyName, explorer);
    }

    protected virtual bool IsListOfCsharpClassOrPrimitive(Type type)
    {
        var genericType = type.GenericTypeArguments.FirstOrDefault();

        if (genericType == null || !IsCsharpClassOrPrimitive(genericType))
        {
            return false;
        }

        return type.ToString().StartsWith("System.Collections.Generic.IEnumerable`") || type.ToString().StartsWith("System.Collections.Generic.List`");
    }

    protected virtual bool IsCsharpClassOrPrimitive(Type? type)
    {
        if (type == null)
        {
            return false;
        }

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
        return type == typeof(List<SelectListItem>) || type == typeof(IEnumerable<SelectListItem>);
    }
    
    protected virtual bool IsFile(Type type)
    {
        return typeof(IFormFile).IsAssignableFrom(type) || 
               typeof(IEnumerable<IFormFile>).IsAssignableFrom(type);
    }

    protected virtual bool IsSelectGroup(TagHelperContext context, ModelExpression model)
    {
        return IsEnum(model.ModelExplorer) || AreSelectItemsProvided(model.ModelExplorer);
    }

    protected virtual bool IsDateGroup(TagHelperContext context, ModelExpression model)
    {
        if (model.ModelExplorer.GetAttribute<DatePickerAttribute>() != null || 
            model.ModelExplorer.GetAttribute<DateRangePickerAttribute>() != null)
        {
            return true;
        }

        if (model.Metadata.ModelType == typeof(DateTime) || 
            model.Metadata.ModelType == typeof(DateTime?) || 
            model.Metadata.ModelType == typeof(DateTimeOffset) || 
            model.Metadata.ModelType == typeof(DateTimeOffset?))
        {
            return true;
        }

        return false;
    }

    protected virtual bool IsEnum(ModelExplorer explorer)
    {
        return explorer.Metadata.IsEnum;
    }

    protected virtual bool AreSelectItemsProvided(ModelExplorer explorer)
    {
        return explorer.GetAttribute<SelectItems>() != null;
    }

    protected virtual bool IsRadioGroup(ModelExplorer explorer)
    {
        return explorer.GetAttribute<AbpRadioButton>() != null;
    }
}
