using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpDynamicFormTagHelperService : AbpTagHelperService<AbpDynamicFormTagHelper>
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly AbpInputTagHelper _abpInputTagHelper;

        public AbpDynamicFormTagHelperService(HtmlEncoder htmlEncoder, AbpInputTagHelper abpInputTagHelper)
        {
            _htmlEncoder = htmlEncoder;
            _abpInputTagHelper = abpInputTagHelper;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var list = new List<InputGroupContent>();
            context.Items.Add("InputGroupContents", list);

            output.TagName = "form";
            output.Attributes.Add("method", "post");
            output.Attributes.Add("action", "#");


            await output.GetChildContentAsync();
            ProcessFields(context, output);

            foreach (var itemConfig in list.OrderBy(o => o.Order))
            {
                output.PostContent.SetHtmlContent(output.PostContent.GetContent() + itemConfig.Html);
            }
        }

        protected virtual void ProcessFields(TagHelperContext context, TagHelperOutput output)
        {
            var models = GetModels(context, output);

            foreach (var model in models)
            {
                ProcessInputGroup(context, model);
            }
        }

        protected virtual List<ModelExpression> GetModels(TagHelperContext context, TagHelperOutput output)
        {
            return ExploreModelsRecursively(new List<ModelExpression>(), TagHelper.Model.ModelExplorer);
        }

        protected virtual void ProcessInputGroup(TagHelperContext context, ModelExpression model)
        {
            _abpInputTagHelper.AspFor = model;
            _abpInputTagHelper.Label = "";
            _abpInputTagHelper.ViewContext = TagHelper.ViewContext;

            RenderTagHelper(new TagHelperAttributeList(), context, _abpInputTagHelper, _htmlEncoder, "div", TagMode.StartTagAndEndTag);
        }

        protected virtual List<ModelExpression> ExploreModelsRecursively(List<ModelExpression> list, ModelExplorer model)
        {
            if (IsCsharpClassOrPrimitive(model.ModelType))
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

        protected virtual bool IsCsharpClassOrPrimitive(Type type)
        {
            return type.IsPrimitive ||
                   type.IsValueType ||
                   type == typeof(DateTime) ||
                   type == typeof(ValueType) ||
                   type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(double) ||
                   type == typeof(Guid) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(float) ||
                   type == typeof(short) ||
                   type == typeof(int) ||
                   type == typeof(long) ||
                   type.IsEnum;
        }
    }
}