using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers
{
    public abstract class AbpTagHelperService<TTagHelper> : IAbpTagHelperService<TTagHelper>
        where TTagHelper : TagHelper
    {
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

        protected TagHelperOutput GetInnerTagHelper(TagHelperAttributeList attributeList, TagHelperContext context, TagHelper tagHelper, string tagName = "div", TagMode tagMode = TagMode.SelfClosing)
        {
            var innerOutput = new TagHelperOutput(tagName, attributeList, (useCachedResult, encoder) => Task.Run<TagHelperContent>(() => new DefaultTagHelperContent()))
            {
                TagMode = tagMode
            };

            var innerContext = new TagHelperContext(attributeList, context.Items, Guid.NewGuid().ToString());

            tagHelper.Process(innerContext, innerOutput);

            return innerOutput;
        }

        protected string RenderTagHelper(TagHelperAttributeList attributeList, TagHelperContext context, TagHelper tagHelper, HtmlEncoder htmlEncoder, string tagName = "div", TagMode tagMode = TagMode.SelfClosing)
        {
            var innerOutput = new TagHelperOutput(tagName, attributeList, (useCachedResult, encoder) => Task.Run<TagHelperContent>(() => new DefaultTagHelperContent()))
            {
                TagMode = tagMode
            };

            var innerContext = new TagHelperContext(attributeList, context.Items, Guid.NewGuid().ToString());

            tagHelper.Process(innerContext, innerOutput);

            return RenderTagHelperOutput(innerOutput, htmlEncoder);
        }

        protected string RenderTagHelperOutput(TagHelperOutput output, HtmlEncoder htmlEncoder)
        {
            using (var writer = new StringWriter())
            {
                output.WriteTo(writer, htmlEncoder);
                return writer.ToString();
            }
        }
        
        public static T GetAttribute<T>(ModelExplorer property) where T : Attribute
        {
            var xd1 = property.Container.ModelType.GetTypeInfo();
            var xd2 = xd1.GetProperty(property.Metadata.PropertyName);

            if (xd2 == null)
            {
                return null;
            }

            var attribute = xd2 .GetCustomAttribute<T>();

            return attribute;
        }
    }
}