using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse
{
    public class AbpAccordionTagHelperService : AbpTagHelperService<AbpAccordionTagHelper>
    {
        protected IHtmlGenerator HtmlGenerator { get; }

        public AbpAccordionTagHelperService(IHtmlGenerator htmlGenerator)
        {
            HtmlGenerator = htmlGenerator;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetRandomIdIfNotProvided();

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AddClass("accordion");
            output.Attributes.Add("id",TagHelper.Id);

            var items = InitilizeFormGroupContentsContext(context, output);

            await output.GetChildContentAsync();

            SetContent(context, output, items);
        }

        protected virtual void SetContent(TagHelperContext context, TagHelperOutput output, List<string> items)
        {
            foreach (var item in items)
            {
                var content = item.Replace(AbpAccordionParentIdPlaceholder, HtmlGenerator.Encode(TagHelper.Id));

                var wrapper = new TagBuilder("div");
                wrapper.AddCssClass("card");
                wrapper.InnerHtml.AppendHtml(content);

                output.Content.AppendHtml(wrapper);
            }
        }

        protected virtual List<string> InitilizeFormGroupContentsContext(TagHelperContext context, TagHelperOutput output)
        {
            var items = new List<string>();
            context.Items[AccordionItems] = items;
            return items;
        }

        protected virtual void SetRandomIdIfNotProvided()
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Id))
            {
                TagHelper.Id = "A" + Guid.NewGuid().ToString("N");
            }
        }
    }
}
