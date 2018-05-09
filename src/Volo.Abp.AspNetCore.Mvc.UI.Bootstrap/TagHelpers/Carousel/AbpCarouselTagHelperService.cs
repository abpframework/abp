using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Carousel
{
    public class AbpCarouselTagHelperService : AbpTagHelperService<AbpCarouselTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            SetRandomIdIfNotProvided();
            AddBasicAttributes(context, output);

            var itemList = InitilizeCarouselItemsContentsContext(context, output);

            await output.GetChildContentAsync();

            SetOneItemAsActive(context, output, itemList);
            SetItems(context, output, itemList);
            SetControls(context, output, itemList);
            SetIndicators(context, output, itemList);
        }


        protected virtual List<CarouselItem> InitilizeCarouselItemsContentsContext(TagHelperContext context, TagHelperOutput output)
        {
            var items = new List<CarouselItem>();
            context.Items[CarouselItemsContent] = items;
            return items;
        }

        protected virtual void SetItems(TagHelperContext context, TagHelperOutput output, List<CarouselItem> itemList)
        {
            var itemsHtml = new StringBuilder("");

            foreach (var carouselItem in itemList)
            {
                SetActiveIfActive(carouselItem);

                itemsHtml.AppendLine(carouselItem.Html);
            }

            output.Content.SetHtmlContent(itemsHtml.ToString());
        }

        protected virtual void SetControls(TagHelperContext context, TagHelperOutput output, List<CarouselItem> itemList)
        {
            var html = new StringBuilder("");

            html.AppendLine("<a class=\"carousel-control-prev\" href=\"#" + TagHelper.Id + "\" role=\"button\" data-slide=\"prev\">");
            html.AppendLine("<span class=\"carousel-control-prev-icon\" aria-hidden=\"true\"></span>");
            html.AppendLine("<span class=\"sr-only\">Previous</span>");
            html.AppendLine("</a>");
            html.AppendLine("<a class=\"carousel-control-next\" href=\"#" + TagHelper.Id + "\" role=\"button\" data-slide=\"next\">");
            html.AppendLine("<span class=\"carousel-control-next-icon\" aria-hidden=\"true\"></span>");
            html.AppendLine("<span class=\"sr-only\">Next</span>");
            html.AppendLine("</a>");

            output.PostContent.SetHtmlContent(html.ToString());
        }

        protected virtual void SetIndicators(TagHelperContext context, TagHelperOutput output, List<CarouselItem> itemList)
        {
            if (TagHelper.Indicators??false)
            {
                return;
            }

            var html = new StringBuilder("<ol class=\"carousel-indicators\">");

            for (var i = 0; i < itemList.Count; i++)
            {
                html.AppendLine(
                    "<li " +
                    "data-target=\"#"+TagHelper.Id+"\"" +
                    " data-slide-to=\""+i+"\"" +
                    (itemList[i].Active?" class=\"active\">":"") +
                    "</li>");
            }

            html.AppendLine("</ol>");
            output.PreContent.SetHtmlContent(html.ToString());
        }

        protected virtual void SetOneItemAsActive(TagHelperContext context, TagHelperOutput output, List<CarouselItem> itemList)
        {
            if (!itemList.Any(it=> it.Active) && itemList.Count > 0)
            {
                itemList.FirstOrDefault().Active = true;
            }
        }

        protected virtual void AddBasicAttributes(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-ride", "carousel");
            output.Attributes.Add("id", TagHelper.Id);
            AddBasicClasses(context, output);
        }

        protected virtual void AddBasicClasses(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("carousel");
            output.Attributes.AddClass("slide");
            SetFadeAnimation(context, output);
        }

        protected virtual void SetFadeAnimation(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Crossfade??false)
            {
                output.Attributes.AddClass("carousel-fade");
            }
        }

        protected virtual void SetRandomIdIfNotProvided()
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Id))
            {
                TagHelper.Id = Guid.NewGuid().ToString("N");
            }
        }

        protected virtual void SetActiveIfActive(CarouselItem item)
        {
            item.Html = item.Html.Replace(AbpCarouselItemActivePlaceholder, item.Active ? "active" : "");
        }

    }
}