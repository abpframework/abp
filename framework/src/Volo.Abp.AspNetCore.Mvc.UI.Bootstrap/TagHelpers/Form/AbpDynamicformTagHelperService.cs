using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpDynamicFormTagHelperService : AbpTagHelperService<AbpDynamicFormTagHelper>
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly IHtmlGenerator _htmlGenerator;
        private readonly IServiceProvider _serviceProvider;

        public AbpDynamicFormTagHelperService(
            HtmlEncoder htmlEncoder,
            IHtmlGenerator htmlGenerator,
            IServiceProvider serviceProvider)
        {
            _htmlEncoder = htmlEncoder;
            _htmlGenerator = htmlGenerator;
            _serviceProvider = serviceProvider;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var list = InitilizeFormGroupContentsContext(context, output);

            NormalizeTagMode(context, output);

            var childContent = (await output.GetChildContentAsync()).GetContent();

            await ConvertToMvcForm(context, output);

            await ProcessFieldsAsync(context, output);

            SetContent(context, output, list, childContent);

            SetFormAttributes(context, output);

            SetSubmitButton(context, output);
        }

        protected virtual async Task ConvertToMvcForm(TagHelperContext context, TagHelperOutput output)
        {
            var formTagHelper = new FormTagHelper(_htmlGenerator)
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

            output.PostContent.SetHtmlContent(output.PostContent.GetContent() + formTagOutput.PostContent.GetContent());
            output.PreContent.SetHtmlContent(output.PreContent.GetContent() + formTagOutput.PreContent.GetContent());
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

        protected virtual void SetContent(TagHelperContext context, TagHelperOutput output, List<FormGroupItem> items, string childContent)
        {
            var contentBuilder = new StringBuilder("");

            foreach (var item in items.OrderBy(o => o.Order))
            {
                contentBuilder.AppendLine(item.HtmlContent);
            }

            if (childContent.Contains(AbpFormContentPlaceHolder))
            {
                childContent = childContent.Replace(AbpFormContentPlaceHolder, contentBuilder.ToString());
            }
            else
            {
                childContent = contentBuilder + childContent;
            }

            output.Content.SetHtmlContent(childContent);
        }

        protected virtual void SetSubmitButton(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.SubmitButton ?? true)
            {
                return;
            }

            var buttonHtml = ProcessSubmitButtonAndGetContentAsync(context, output);

            output.PostContent.SetHtmlContent(output.PostContent.GetContent() + buttonHtml);
        }

        protected virtual List<FormGroupItem> InitilizeFormGroupContentsContext(TagHelperContext context, TagHelperOutput output)
        {
            var items = new List<FormGroupItem>();
            context.Items[FormGroupContents] = items;
            return items;
        }

        protected virtual async Task ProcessFieldsAsync(TagHelperContext context, TagHelperOutput output)
        {
            var models = GetModels(context, output);

            foreach (var model in models)
            {
                if (IsSelectGroup(context, model))
                {
                    await ProcessSelectGroupAsync(context, output, model);
                }
                else
                {
                    await ProcessInputGroupAsync(context, output, model);
                }
            }
        }

        protected virtual async Task ProcessSelectGroupAsync(TagHelperContext context, TagHelperOutput output, ModelExpression model)
        {
            var abpSelectTagHelper = GetSelectGroupTagHelper(context, output, model);

            await abpSelectTagHelper.RenderAsync(new TagHelperAttributeList(), context, _htmlEncoder, "div", TagMode.StartTagAndEndTag);
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
            var radioButtonAttribute = model.ModelExplorer.GetAttribute<AbpRadioButton>();
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
            abpButtonTagHelper.Text = "Submit";
            abpButtonTagHelper.ButtonType = AbpButtonType.Primary;

            return await abpButtonTagHelper.RenderAsync(attributes, context, _htmlEncoder, "button", TagMode.StartTagAndEndTag);
        }

        protected virtual async Task ProcessInputGroupAsync(TagHelperContext context, TagHelperOutput output, ModelExpression model)
        {
            var abpInputTagHelper = _serviceProvider.GetRequiredService<AbpInputTagHelper>();
            abpInputTagHelper.AspFor = model;
            abpInputTagHelper.ViewContext = TagHelper.ViewContext;
            abpInputTagHelper.DisplayRequiredSymbol = TagHelper.RequiredSymbols ?? true;

            await abpInputTagHelper.RenderAsync(new TagHelperAttributeList(), context, _htmlEncoder, "div", TagMode.StartTagAndEndTag);
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

        protected virtual bool IsCsharpClassOrPrimitive(Type type)
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

        protected virtual bool IsSelectGroup(TagHelperContext context, ModelExpression model)
        {
            return IsEnum(model.ModelExplorer) || AreSelectItemsProvided(model.ModelExplorer);
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
}