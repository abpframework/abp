using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse
{
    public class AbpAccordionTagHelperService : AbpTagHelperService<AbpAccordionTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetRandomIdIfNotProvided();

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AddClass("accordion");
            output.Attributes.Add("id",TagHelper.Id);
            
            var items = InitilizeFormGroupContentsContext(context, output);

            await output.GetChildContentAsync();

            var content = GetContent(items);

            output.Content.SetHtmlContent(content);
        }

        protected virtual string GetContent(List<string> items)
        {
            var html = new StringBuilder("");
            foreach (var item in items)
            {
                var content = item.Replace(AbpAccordionParentIdPlaceholder, TagHelper.Id);

                html.AppendLine(
                    "<div class=\"card\">" + Environment.NewLine + 
                        content
                    + "</div>" + Environment.NewLine
                );
            }

            return html.ToString();
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