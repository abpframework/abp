using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Carousel
{
    public class AbpCarouselItemTagHelperService : AbpTagHelperService<AbpCarouselItemTagHelper>
    {
        private readonly HtmlEncoder _encoder;

        public AbpCarouselItemTagHelperService(HtmlEncoder encoder)
        {
            _encoder = encoder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("carousel-item");
            output.Attributes.AddClass(AbpCarouselItemActivePlaceholder);

            SetInnerImgTag(context, output);
            SetActive(context, output);
            AddCaption(context, output);

            AddToContext(context, output);

            output.SuppressOutput();
        }

        private void AddToContext(TagHelperContext context, TagHelperOutput output)
        {
            var getOutputAsHtml = output.Render(_encoder);

            var itemList = context.GetValue<List<CarouselItem>>(CarouselItemsContent);

            itemList.Add(new CarouselItem(getOutputAsHtml, TagHelper.Active ?? false));
        }

        protected virtual void SetInnerImgTag(TagHelperContext context, TagHelperOutput output)
        {
            var imgTag ="<img class=\"d-block w-100\" src=\""+TagHelper.Src+ "\" alt=\"" + TagHelper.Alt + "\">";
            output.Content.SetHtmlContent(imgTag);
        }

        protected virtual void SetActive(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Active??false)
            {
                output.Attributes.AddClass("active");
            }
        }

        protected virtual void AddCaption(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Caption) && string.IsNullOrWhiteSpace(TagHelper.CaptionTitle))
            {
                return;
            }

            var html = new StringBuilder("");

            html.AppendLine("<div class=\"carousel-caption d-none d-md-block\">");
            html.AppendLine("<h5>"+TagHelper.CaptionTitle+"</h5>");
            html.AppendLine("<p>" + TagHelper.Caption + "</p>");
            html.AppendLine("</div>");

            output.PostContent.SetHtmlContent(html.ToString());
        }

    }
}