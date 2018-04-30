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
        private const string SelectItemsPostFix = "selectitems";

        public AbpDynamicFormTagHelperService(HtmlEncoder htmlEncoder, AbpInputTagHelper abpInputTagHelper)
        {
            _htmlEncoder = htmlEncoder;
            _abpInputTagHelper = abpInputTagHelper;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "form";
            output.Attributes.Add("method", "post");
            output.Attributes.Add("action", "#");

            var inputsAsHtml = await GetInputsAsHtml(output);

            output.PreContent.SetHtmlContent(inputsAsHtml);
        }

        protected virtual async Task<string> GetInputsAsHtml(TagHelperOutput output)
        {
            var inputsAsHtml = "";
            var models = await GetModels(output);

            foreach (var model in models)
            {
                inputsAsHtml += ProcessInputGroup(model);
            }

            return inputsAsHtml;
        }

        protected virtual async Task<List<ModelExpression>> GetModels(TagHelperOutput output)
        {

            var models = ExploreModelsRecursively(new List<ModelExpression>(), TagHelper.Model.ModelExplorer);

            return await RemoveOverridenFieldsAndGetModels(output, models);
        }

        protected virtual string ProcessInputGroup(ModelExpression model)
        {
            _abpInputTagHelper.AspFor = model;
            _abpInputTagHelper.Label = "";
            _abpInputTagHelper.ViewContext = TagHelper.ViewContext;

            return RenderTagHelper(new TagHelperAttributeList(), _abpInputTagHelper, _htmlEncoder, "div", TagMode.StartTagAndEndTag)
                    + Environment.NewLine;
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

        protected virtual async Task<List<ModelExpression>> RemoveOverridenFieldsAndGetModels(TagHelperOutput output, List<ModelExpression> models)
        {
            var overridenFormElementsProcessed = (await output.GetChildContentAsync()).GetContent();
            return models.Where(m => !overridenFormElementsProcessed.Contains("id=\"" + m.Name.Replace('.','_') + "\"")).ToList();
        }
    }
}