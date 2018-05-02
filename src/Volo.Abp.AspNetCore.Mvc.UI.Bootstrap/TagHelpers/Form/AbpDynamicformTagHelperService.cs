using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var list = InitilizeFormGroupContentsContext(context);

            NormalizeTagMode(context, output);
            
            await output.GetChildContentAsync();

            await ConvertToMvcForm(context, output);

            ProcessFields(context, output);

            SetContent(output,list);

            SetFormAttributes(output);

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
            
            var formTagOutput = GetInnerTagHelper(output.Attributes, context, formTagHelper, "form", TagMode.StartTagAndEndTag);

            await formTagOutput.GetChildContentAsync();

            output.PostContent.SetHtmlContent(output.PostContent.GetContent() + formTagOutput.PostContent.GetContent());
            output.PreContent.SetHtmlContent(output.PreContent.GetContent() + formTagOutput.PreContent.GetContent());
        }

        protected virtual void NormalizeTagMode(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "form";
        }

        protected virtual void SetFormAttributes(TagHelperOutput output)
        {
            output.Attributes.AddIfNotContains("method", "post");
        }

        protected virtual void SetContent(TagHelperOutput output, List<FormGroupItem> items)
        {
            var preContentBuilder = new StringBuilder(output.PreContent.GetContent());

            foreach (var item in items.OrderBy(o => o.Order))
            {
                preContentBuilder.AppendLine(item.HtmlContent);
            }

            output.PreContent.SetHtmlContent(preContentBuilder.ToString());
        }

        protected virtual void SetSubmitButton(TagHelperContext context, TagHelperOutput output)
        {
            if (!TagHelper.SubmitButton ?? true)
            {
                return;
            }

            var buttonHtml = ProcessSubmitButtonAndGetContent(context, output);

            output.PostContent.SetHtmlContent(output.PostContent.GetContent() + buttonHtml);
        }

        protected virtual List<FormGroupItem> InitilizeFormGroupContentsContext(TagHelperContext context)
        {
            var items = new List<FormGroupItem>();
            context.Items[FormGroupContents] = items;
            return items;
        }
        
        protected virtual void ProcessFields(TagHelperContext context, TagHelperOutput output)
        {
            var models = GetModels(context, output);

            foreach (var model in models)
            {
                if (IsSelectGroup(context, model))
                {
                    ProcessSelectGroup(context, model);
                    continue;
                }

                ProcessInputGroup(context, model);
            }
        }

        protected virtual void ProcessSelectGroup(TagHelperContext context, ModelExpression model)
        {
            var abpSelectTagHelper = _serviceProvider.GetRequiredService<AbpSelectTagHelper>();
            abpSelectTagHelper.AspFor = model;
            abpSelectTagHelper.AspItems = null;
            abpSelectTagHelper.Label = "";
            abpSelectTagHelper.ViewContext = TagHelper.ViewContext;

            RenderTagHelper(new TagHelperAttributeList(), context, abpSelectTagHelper, _htmlEncoder, "div", TagMode.StartTagAndEndTag);
        }

        protected virtual string ProcessSubmitButtonAndGetContent(TagHelperContext context, TagHelperOutput output)
        {
            var abpButtonTagHelper = _serviceProvider.GetRequiredService<AbpButtonTagHelper>();
            var attributes = new TagHelperAttributeList { new TagHelperAttribute("type", "submit") };
            abpButtonTagHelper.Text = "Submit";
            abpButtonTagHelper.ButtonType = AbpButtonType.Primary;

            return RenderTagHelper(attributes, context, abpButtonTagHelper, _htmlEncoder, "button", TagMode.StartTagAndEndTag);
        }

        protected virtual void ProcessInputGroup(TagHelperContext context, ModelExpression model)
        {
            var abpInputTagHelper = _serviceProvider.GetRequiredService<AbpInputTagHelper>();
            abpInputTagHelper.AspFor = model;
            abpInputTagHelper.Label = "";
            abpInputTagHelper.ViewContext = TagHelper.ViewContext;

            RenderTagHelper(new TagHelperAttributeList(), context, abpInputTagHelper, _htmlEncoder, "div", TagMode.StartTagAndEndTag);
        }

        protected virtual List<ModelExpression> GetModels(TagHelperContext context, TagHelperOutput output)
        {
            return TagHelper.Model.ModelExplorer.Properties.Aggregate(new List<ModelExpression>(), ExploreModelsRecursively);
        }

        protected virtual List<ModelExpression> ExploreModelsRecursively(List<ModelExpression> list, ModelExplorer model)
        {
            if (IsCsharpClassOrPrimitive(model.ModelType))
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

        protected virtual bool IsCsharpClassOrPrimitive(Type type)
        {
            return type.IsPrimitive ||
                   type.IsValueType ||
                   type == typeof(DateTime) ||
                   type == typeof(ValueType) ||
                   type == typeof(String) ||
                   type == typeof(Decimal) ||
                   type == typeof(Double) ||
                   type == typeof(Guid) ||
                   type == typeof(Char) ||
                   type == typeof(Byte) ||
                   type == typeof(Boolean) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(Int16) ||
                   type == typeof(Int32) ||
                   type == typeof(Int64) ||
                   type == typeof(ushort) ||
                   type == typeof(uint) ||
                   type == typeof(ulong) ||
                   type == typeof(float) ||
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
            return GetAttribute<SelectItems>(explorer) != null;
        }
    }
}