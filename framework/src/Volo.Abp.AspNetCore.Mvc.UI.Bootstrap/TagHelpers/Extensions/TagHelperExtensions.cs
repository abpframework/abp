using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions
{
    public static class TagHelperExtensions
    {
        public static async Task<TagHelperOutput> ProcessAndGetOutputAsync(
            this TagHelper tagHelper, 
            TagHelperAttributeList attributeList, 
            TagHelperContext context, 
            string tagName = "div", 
            TagMode tagMode = TagMode.SelfClosing)
        {
            var innerOutput = new TagHelperOutput(
                tagName,
                attributeList,
                (useCachedResult, encoder) => Task.Run<TagHelperContent>(() => new DefaultTagHelperContent()))
            {
                TagMode = tagMode
            };
            
            var innerContext = new TagHelperContext(
                attributeList,
                context.Items,
                Guid.NewGuid().ToString()
            );

            tagHelper.Init(context);

            await tagHelper.ProcessAsync(innerContext, innerOutput);

            return innerOutput;
        }

        public static async Task<string> RenderAsync(this TagHelper tagHelper, TagHelperAttributeList attributeList, TagHelperContext context, HtmlEncoder htmlEncoder, string tagName = "div", TagMode tagMode = TagMode.SelfClosing)
        {
            var innerOutput = await tagHelper.ProcessAndGetOutputAsync(attributeList, context, tagName, tagMode);

            return innerOutput.Render(htmlEncoder);
        }
    }
}
