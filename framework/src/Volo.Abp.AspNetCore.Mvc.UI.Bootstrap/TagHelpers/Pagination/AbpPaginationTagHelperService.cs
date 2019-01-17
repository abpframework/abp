using System.Text;
using System.Text.Encodings.Web;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination
{
    public class AbpPaginationTagHelperService : AbpTagHelperService<AbpPaginationTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;
        private readonly IAbpTagHelperLocalizer _tagHelperLocalizer;

        public AbpPaginationTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder, IAbpTagHelperLocalizer tagHelperLocalizer)
        {
            _generator = generator;
            _encoder = encoder;
            _tagHelperLocalizer = tagHelperLocalizer;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Model.ShownItemsCount <= 0)
            {
                output.SuppressOutput();
            }

            ProcessMainTag(context, output);
            SetContentAsHtml(context, output);
        }

        protected virtual void SetContentAsHtml(TagHelperContext context, TagHelperOutput output)
        {
            var html = new StringBuilder("");

            html.AppendLine(GetOpeningTags(context, output));
            html.AppendLine(GetPreviousButton(context, output));
            html.AppendLine(GetPages(context, output));
            html.AppendLine(GetNextButton(context, output));
            html.AppendLine(GetClosingTags(context, output));

            output.Content.SetHtmlContent(html.ToString());
        }

        protected virtual void ProcessMainTag(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.AddClass("row");
            output.Attributes.AddClass("mt-3");
        }

        protected virtual string GetPages(TagHelperContext context, TagHelperOutput output)
        {
            var pagesHtml = new StringBuilder("");

            foreach (var page in TagHelper.Model.Pages)
            {
                pagesHtml.AppendLine(GetPage(context, output, page));
            }

            return pagesHtml.ToString();
        }

        protected virtual string GetPage(TagHelperContext context, TagHelperOutput output, PageItem page)
        {
            var pageHtml = new StringBuilder("");

            pageHtml.AppendLine("<li class=\"page-item " + (TagHelper.Model.CurrentPage == page.Index ? "active" : "") + "\">");

            if (page.IsGap)
            {
                pageHtml.AppendLine("<span class=\"page-link gap\">…</span>");
            }
            else if(!page.IsGap && TagHelper.Model.CurrentPage == page.Index)
            {
                pageHtml.AppendLine("                                 <span class=\"page-link\">\r\n" +
                                    "                                    " + page.Index + "\r\n" +
                                    "                                    <span class=\"sr-only\">(current)</span>\r\n" +
                                    "                                </span>");
            }
            else
            {
                pageHtml.AppendLine(RenderAnchorTagHelperLinkHtml(context, output, page.Index.ToString(), page.Index.ToString()));
            }

            pageHtml.AppendLine("</li>");

            return pageHtml.ToString();
        }

        protected virtual string GetPreviousButton(TagHelperContext context, TagHelperOutput output)
        {
            var localizationKey = "PagerPrevious";
            var currentPage = TagHelper.Model.CurrentPage == 1
                ? TagHelper.Model.CurrentPage.ToString()
                : (TagHelper.Model.CurrentPage - 1).ToString();
            return
                "<li class=\"page-item " + (TagHelper.Model.CurrentPage == 1 ? "disabled" : "") + "\">\r\n" +
                RenderAnchorTagHelperLinkHtml(context, output, currentPage, localizationKey) +
                "                </li>";
        }

        protected virtual string GetNextButton(TagHelperContext context, TagHelperOutput output)
        {
            var localizationKey = "PagerNext";
            var currentPage = (TagHelper.Model.CurrentPage + 1).ToString();
            return
                "<li class=\"page-item " + (TagHelper.Model.CurrentPage >= TagHelper.Model.TotalPageCount ? "disabled" : "") + "\">\r\n" +
                RenderAnchorTagHelperLinkHtml(context, output, currentPage, localizationKey) +
                "                </li>";
        }

        protected virtual string RenderAnchorTagHelperLinkHtml(TagHelperContext context, TagHelperOutput output, string currentPage, string localizationKey)
        {
            var localizer = _tagHelperLocalizer.GetLocalizer(typeof(AbpUiResource));

            var anchorTagHelper = GetAnchorTagHelper(currentPage, out var attributeList);

            var tagHelperOutput = GetInnerTagHelper(attributeList, context, anchorTagHelper, "a", TagMode.StartTagAndEndTag);

            tagHelperOutput.Content.SetHtmlContent(localizer[localizationKey]);

            var renderedHtml = RenderTagHelperOutput(tagHelperOutput, _encoder);

            return renderedHtml;
        }

        private AnchorTagHelper GetAnchorTagHelper(string currentPage, out TagHelperAttributeList attributeList)
        {
            var anchorTagHelper = new AnchorTagHelper(_generator)
            {
                Page = TagHelper.Model.PageUrl,
                ViewContext = TagHelper.ViewContext
            };

            anchorTagHelper.RouteValues.Add("currentPage", currentPage);
            anchorTagHelper.RouteValues.Add("sort", TagHelper.Model.Sort);

            attributeList = new TagHelperAttributeList
            {
                new TagHelperAttribute("tabindex", "-1"),
                new TagHelperAttribute("class", "page-link")
            };

            return anchorTagHelper;
        }

        protected virtual string GetOpeningTags(TagHelperContext context, TagHelperOutput output)
        {
            var localizer = _tagHelperLocalizer.GetLocalizer(typeof(AbpUiResource));

            var pagerInfo = (TagHelper.ShowInfo ?? false) ?
                "    <div class=\"col-sm-12 col-md-5\"> " + localizer["PagerInfo", TagHelper.Model.ShowingFrom, TagHelper.Model.ShowingTo, TagHelper.Model.TotalItemsCount] + "</div>\r\n"
                : "";

            return
                pagerInfo +
                "    <div class=\"col-sm-12 col-md-7\">\r\n" +
                "        <nav aria-label=\"Page navigation\">\r\n" +
                "            <ul class=\"pagination justify-content-end\">";
        }

        protected virtual string GetClosingTags(TagHelperContext context, TagHelperOutput output)
        {
            return
                "            </ul>\r\n" +
                "         </ nav>\r\n" +
                "    </div>\r\n";
        }
    }
}