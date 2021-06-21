using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Carousel
{
    public class AbpCarouselTagHelperService : AbpTagHelperService<AbpCarouselTagHelper>
    {
        protected IStringLocalizer<AbpUiResource> L { get; }

        public AbpCarouselTagHelperService(IStringLocalizer<AbpUiResource> localizer)
        {
            L = localizer;
        }

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
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("carousel-inner");

            foreach (var carouselItem in itemList)
            {
                SetActiveIfActive(carouselItem);

                wrapper.InnerHtml.AppendHtml(carouselItem.Html);
            }

            output.Content.SetHtmlContent(wrapper);
        }

        protected virtual void SetControls(TagHelperContext context, TagHelperOutput output, List<CarouselItem> itemList)
        {
            if (!TagHelper.Controls ?? false)
            {
                return;
            }

            // create 'previous' item
            var prevIcon = new TagBuilder("span");
            prevIcon.AddCssClass("carousel-control-prev-icon");
            prevIcon.Attributes.Add("aria-hidden", "true");

            var prevText = new TagBuilder("span");
            prevText.AddCssClass("sr-only");
            prevText.InnerHtml.Append(L["Previous"].Value);

            var prevAnchor = new TagBuilder("a");
            prevAnchor.AddCssClass("carousel-control-prev");
            prevAnchor.Attributes.Add("href", "#" + TagHelper.Id);
            prevAnchor.Attributes.Add("role", "button");
            prevAnchor.Attributes.Add("data-slide", "prev");

            prevAnchor.InnerHtml.AppendHtml(prevIcon);
            prevAnchor.InnerHtml.AppendHtml(prevText);

            // create 'next' item
            var nextIcon = new TagBuilder("span");
            nextIcon.AddCssClass("carousel-control-next-icon");
            nextIcon.Attributes.Add("aria-hidden", "true");

            var nextText = new TagBuilder("span");
            nextText.AddCssClass("sr-only");
            nextText.InnerHtml.Append(L["Next"].Value);

            var nextAnchor = new TagBuilder("a");
            nextAnchor.AddCssClass("carousel-control-next");
            nextAnchor.Attributes.Add("href", "#" + TagHelper.Id);
            nextAnchor.Attributes.Add("role", "button");
            nextAnchor.Attributes.Add("data-slide", "next");

            nextAnchor.InnerHtml.AppendHtml(nextIcon);
            nextAnchor.InnerHtml.AppendHtml(nextText);

            // append post content
            output.PostContent.AppendHtml(prevAnchor);
            output.PostContent.AppendHtml(nextAnchor);
        }

        protected virtual void SetIndicators(TagHelperContext context, TagHelperOutput output, List<CarouselItem> itemList)
        {
            if (!TagHelper.Indicators ?? false)
            {
                return;
            }

            var list = new TagBuilder("ol");
            list.AddCssClass("carousel-indicators");

            for (var i = 0; i < itemList.Count; i++)
            {
                var listItem = new TagBuilder("li");
                listItem.Attributes.Add("data-target", "#" + TagHelper.Id);
                listItem.Attributes.Add("data-slide-to", i.ToString());

                if (itemList[i].Active)
                {
                    listItem.AddCssClass("active");
                }

                list.InnerHtml.AppendHtml(listItem);
            }

            output.PreContent.SetHtmlContent(list);
        }

        protected virtual void SetOneItemAsActive(TagHelperContext context, TagHelperOutput output, List<CarouselItem> itemList)
        {
            if (!itemList.Any(it => it.Active) && itemList.Count > 0)
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
            if (TagHelper.Crossfade ?? false)
            {
                output.Attributes.AddClass("carousel-fade");
            }
        }

        protected virtual void SetRandomIdIfNotProvided()
        {
            if (string.IsNullOrWhiteSpace(TagHelper.Id))
            {
                TagHelper.Id = "C" + Guid.NewGuid().ToString("N");
            }
        }

        protected virtual void SetActiveIfActive(CarouselItem item)
        {
            item.Html = item.Html.Replace(AbpCarouselItemActivePlaceholder, item.Active ? "active" : "");
        }

    }
}
