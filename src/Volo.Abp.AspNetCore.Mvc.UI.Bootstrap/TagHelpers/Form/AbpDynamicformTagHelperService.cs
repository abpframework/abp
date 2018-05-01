using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpDynamicFormTagHelperService : AbpTagHelperService<AbpDynamicFormTagHelper>
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly IServiceProvider _serviceProvider;

        public AbpDynamicFormTagHelperService(
            HtmlEncoder htmlEncoder, 
            IServiceProvider serviceProvider)
        {
            _htmlEncoder = htmlEncoder;
            _serviceProvider = serviceProvider;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            NormalizeTagMode(context, output);

            SetFormAttributes(output);

            var list = InitilizeFormGroupContentsContext(context);
            
            await output.GetChildContentAsync();

            ProcessFields(context, output);

            SetContent(output,list);
        }

        protected virtual void NormalizeTagMode(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
        }

        protected virtual void SetFormAttributes(TagHelperOutput output)
        {
            output.Attributes.AddIfNotContains("method", "post");
        }

        protected virtual void SetContent(TagHelperOutput output, List<FormGroupItem> list)
        {
            foreach (var itemConfig in list.OrderBy(o => o.Order))
            {
                output.PostContent.SetHtmlContent(output.PostContent.GetContent() + itemConfig.HtmlContent);
            }
        }

        protected virtual List<FormGroupItem> InitilizeFormGroupContentsContext(TagHelperContext context)
        {
            var list = new List<FormGroupItem>();
            context.Items.Add(FormGroupContentsKey, list);
            return list;
        }
        
        protected virtual void ProcessFields(TagHelperContext context, TagHelperOutput output)
        {
            var models = GetModels(context, output);

            foreach (var model in models)
            {
                if (IsSelectGroup(context, model, out var selectItems))
                {
                    ProcessSelectGroup(context, model, selectItems);
                    continue;
                }

                ProcessInputGroup(context, model);
            }
        }

        protected virtual void ProcessSelectGroup(TagHelperContext context, ModelExpression model, IEnumerable<SelectListItem> selectItems)
        {
            var abpSelectTagHelper = _serviceProvider.GetRequiredService<AbpSelectTagHelper>();
            abpSelectTagHelper.AspFor = model;
            abpSelectTagHelper.AspItems = selectItems;
            abpSelectTagHelper.Label = "";
            abpSelectTagHelper.ViewContext = TagHelper.ViewContext;

            RenderTagHelper(new TagHelperAttributeList(), context, abpSelectTagHelper, _htmlEncoder, "div", TagMode.StartTagAndEndTag);
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

        protected virtual bool IsSelectGroup(TagHelperContext context, ModelExpression model, out IEnumerable<SelectListItem> selectItems)
        {
            return IsEnum(model.ModelExplorer, out selectItems) || AreSelectItemsProvided(model.ModelExplorer, out selectItems);
        }

        protected virtual bool IsEnum(ModelExplorer explorer, out IEnumerable<SelectListItem> selectItems)
        {
            selectItems = explorer.Metadata.IsEnum ? GetSelectItemsFromEnum(explorer.ModelType) : null;
            return explorer.Metadata.IsEnum;
        }

        protected virtual IEnumerable<SelectListItem> GetSelectItemsFromEnum(Type enumType)
        {
            return enumType.GetTypeInfo().GetMembers(BindingFlags.Public | BindingFlags.Static)
                .Select((t, i) => new SelectListItem { Value = i.ToString(), Text = t.Name }).ToList();
        }

        protected virtual bool AreSelectItemsProvided(ModelExplorer explorer, out IEnumerable<SelectListItem> selectItems)
        {
            selectItems = GetAttribute<SelectItems>(explorer)?.GetItems(explorer);
            
            return selectItems != null;
        }
    }
}