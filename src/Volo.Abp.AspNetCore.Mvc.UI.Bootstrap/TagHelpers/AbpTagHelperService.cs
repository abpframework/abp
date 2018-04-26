using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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

        protected string RenderInputTagHelper(TagHelperAttributeList attributeList, AbpTagHelper abpTagHelper, HtmlEncoder htmlEncoder, string tagName = "div")
        {
            var innerOutput = new TagHelperOutput(tagName, attributeList, (useCachedResult, encoder) => Task.Run<TagHelperContent>(() => new DefaultTagHelperContent()))
            {
                TagMode = TagMode.SelfClosing
            };

            var innerContext = new TagHelperContext(attributeList, new Dictionary<object, object>(), Guid.NewGuid().ToString());

            abpTagHelper.Process(innerContext, innerOutput);

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
    }
}