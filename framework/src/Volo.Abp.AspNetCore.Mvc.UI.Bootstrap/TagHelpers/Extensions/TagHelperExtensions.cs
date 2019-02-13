using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions
{
    public static class TagHelperExtensions
    {
        public static TagHelperOutput ProcessAndGetOutput(this TagHelper tagHelper, TagHelperAttributeList attributeList, TagHelperContext context, string tagName = "div", TagMode tagMode = TagMode.SelfClosing, bool runAsync = false)
        {
            var innerOutput = new TagHelperOutput(tagName, attributeList, (useCachedResult, encoder) => Task.Run<TagHelperContent>(() => new DefaultTagHelperContent()))
            {
                TagMode = tagMode
            };

            var innerContext = new TagHelperContext(attributeList, context.Items, Guid.NewGuid().ToString());

            tagHelper.Init(context);

            if (runAsync)
            {
                AsyncHelper.RunSync(() => tagHelper.ProcessAsync(innerContext, innerOutput));
            }
            else
            {
                tagHelper.Process(innerContext, innerOutput);
            }

            return innerOutput;
        }

        public static string Render(this TagHelper tagHelper, TagHelperAttributeList attributeList, TagHelperContext context, HtmlEncoder htmlEncoder, string tagName = "div", TagMode tagMode = TagMode.SelfClosing, bool runAsync = false)
        {
            var innerOutput = tagHelper.ProcessAndGetOutput(attributeList, context, tagName, tagMode, runAsync);

            return innerOutput.Render(htmlEncoder);
        }
    }
}
