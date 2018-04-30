using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
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

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "form";
            output.Attributes.Add("method", "post");
            output.Attributes.Add("action", "#");

            var innerhtml = GetInnerHtmlContent();

            output.PreContent.SetHtmlContent(innerhtml);
        }

        protected virtual string GetInnerHtmlContent()
        {
            var innerHtmlcontent = "";
            var models = ExploreModelsRecursively(new List<ModelExpression>(), TagHelper.Model.ModelExplorer);

            foreach (var model in models)
            {
                innerHtmlcontent += ProcessInputGroup(model);
            }

            return innerHtmlcontent;
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
            return new ModelExpression(explorer.Metadata.PropertyName, explorer);
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
                   type == typeof(TimeSpan) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(Single) ||
                   type == typeof(Int16) ||
                   type == typeof(Int32) ||
                   type == typeof(Int64) ||
                   type.IsEnum;
        }
    }
}