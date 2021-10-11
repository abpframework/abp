using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Breadcrumb
{
    public class AbpBreadcrumbItemTagHelperService : AbpTagHelperService<AbpBreadcrumbItemTagHelper>
    {
        private readonly HtmlEncoder _encoder;

        public AbpBreadcrumbItemTagHelperService(HtmlEncoder encoder)
        {
            _encoder = encoder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "li";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AddClass("breadcrumb-item");
            output.Attributes.AddClass(AbpBreadcrumbItemActivePlaceholder);

            var list = context.GetValue<List<BreadcrumbItem>>(BreadcrumbItemsContent);

            output.Content.SetHtmlContent(GetInnerHtml(context, output));

            list.Add(new BreadcrumbItem
            {
                Html = output.Render(_encoder),
                Active = TagHelper.Active
            });

            output.SuppressOutput();
        }

        protected virtual string GetInnerHtml(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Href))
            {
                output.Attributes.Add("aria-current", "page");
                return _encoder.Encode(TagHelper.Title);
            }

            var link = new TagBuilder("a");
            link.Attributes.Add("href", TagHelper.Href);
            link.InnerHtml.AppendHtml(TagHelper.Title);
            return link.ToHtmlString();
        }
    }
}
