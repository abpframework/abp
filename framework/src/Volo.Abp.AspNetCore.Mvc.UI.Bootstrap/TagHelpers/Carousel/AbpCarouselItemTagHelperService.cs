using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Text.Encodings.Web;
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
            var img = new TagBuilder("img");
            img.AddCssClass("d-block w-100");
            img.Attributes.Add("src", TagHelper.Src);
            img.Attributes.Add("alt", TagHelper.Alt);

            output.Content.SetHtmlContent(img);
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

            var title = new TagBuilder("h5");
            title.InnerHtml.AppendHtml(TagHelper.CaptionTitle);

            var caption = new TagBuilder("p");
            caption.InnerHtml.AppendHtml(TagHelper.Caption);

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("carousel-caption d-none d-md-block");
            wrapper.InnerHtml.AppendHtml(title);
            wrapper.InnerHtml.AppendHtml(caption);

            output.PostContent.SetHtmlContent(wrapper);
        }

    }
}
