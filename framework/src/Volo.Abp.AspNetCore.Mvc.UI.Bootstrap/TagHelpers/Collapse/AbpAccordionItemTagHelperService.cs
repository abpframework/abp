using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse
{
    public class AbpAccordionItemTagHelperService : AbpTagHelperService<AbpAccordionItemTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetRandomIdIfNotProvided();

            var childContent = await output.GetChildContentAsync();

            var html = GetAccordionHeaderItem(context, output) + GetAccordionContentItem(context, output, childContent);

            var tabHeaderItems = context.GetValue<List<string>>(AccordionItems);
            tabHeaderItems.Add(html);

            output.SuppressOutput();
        }

        protected virtual string GetAccordionHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var button = new TagBuilder("button");
            button.AddCssClass("btn btn-link");
            button.Attributes.Add("type", "button");
            button.Attributes.Add("data-toggle", "collapse");
            button.Attributes.Add("data-target", "#" + GetContentId());
            button.Attributes.Add("aria-expanded", "true");
            button.Attributes.Add("aria-controls", GetContentId());
            button.InnerHtml.AppendHtml(TagHelper.Title);

            var h5 = new TagBuilder("h5");
            h5.AddCssClass("mb-0");
            h5.InnerHtml.AppendHtml(button);

            var header = new TagBuilder("div");
            header.AddCssClass("card-header");
            header.Attributes.Add("id", GetHeadingId());
            header.InnerHtml.AppendHtml(h5);

            return header.ToHtmlString();
        }

        protected virtual string GetAccordionContentItem(TagHelperContext context, TagHelperOutput output, TagHelperContent content)
        {
            var show = (TagHelper.Active ?? false) ? " show" : "";

            var cardBody = new TagBuilder("div");
            cardBody.AddCssClass("card-body");
            cardBody.InnerHtml.AppendHtml(content);

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("collapse" + show);
            wrapper.Attributes.Add("id", GetContentId());
            wrapper.Attributes.Add("aria-labelledby", GetHeadingId());
            wrapper.Attributes.Add("data-parent", "#" + AbpAccordionParentIdPlaceholder);
            wrapper.InnerHtml.AppendHtml(cardBody);

            return wrapper.ToHtmlString();
        }

        protected virtual string GetHeadingId()
        {
            return "heading" + TagHelper.Id; ;
        }

        protected virtual string GetContentId()
        {
            return "content" + TagHelper.Id; ;
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
