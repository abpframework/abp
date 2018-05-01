using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public abstract class AbpTagHelperService<TTagHelper> : IAbpTagHelperService<TTagHelper>
        where TTagHelper : TagHelper
    {
        protected const string FormGroupContents = "FormGroupContents";

        public TTagHelper TagHelper { get; set; }

        public virtual int Order { get; }

        public virtual void Init(TagHelperContext context)
        {

        }

        public virtual void Process(TagHelperContext context, TagHelperOutput output)
        {

        }

        public virtual Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Process(context, output);
            return Task.CompletedTask;
        }

        protected virtual TagHelperOutput GetInnerTagHelper(TagHelperAttributeList attributeList, TagHelperContext context, TagHelper tagHelper, string tagName = "div", TagMode tagMode = TagMode.SelfClosing)
        {
            var innerOutput = new TagHelperOutput(tagName, attributeList, (useCachedResult, encoder) => Task.Run<TagHelperContent>(() => new DefaultTagHelperContent()))
            {
                TagMode = tagMode
            };

            var innerContext = new TagHelperContext(attributeList, context.Items, Guid.NewGuid().ToString());

            tagHelper.Process(innerContext, innerOutput);

            return innerOutput;
        }

        protected virtual string RenderTagHelper(TagHelperAttributeList attributeList, TagHelperContext context, TagHelper tagHelper, HtmlEncoder htmlEncoder, string tagName = "div", TagMode tagMode = TagMode.SelfClosing)
        {
            var innerOutput = GetInnerTagHelper(attributeList, context, tagHelper, tagName, tagMode);

            return RenderTagHelperOutput(innerOutput, htmlEncoder);
        }

        protected virtual string RenderTagHelperOutput(TagHelperOutput output, HtmlEncoder htmlEncoder)
        {
            using (var writer = new StringWriter())
            {
                output.WriteTo(writer, htmlEncoder);
                return writer.ToString();
            }
        }

        protected virtual T GetAttribute<T>(ModelExplorer property) where T : Attribute
        {
            return property?.Metadata?.ContainerType?.GetTypeInfo()?.GetProperty(property.Metadata.PropertyName)?.GetCustomAttribute<T>();
        }

        protected virtual List<FormGroupContent> GetFormGroupContentsList(TagHelperContext context)
        {
            return context.Items[FormGroupContents] as List<FormGroupContent>;
        }


    }
}