using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Pagination
{
    public class AbpPaginationTagHelperService : AbpTagHelperService<AbpPaginationTagHelper>
    {
        private readonly IHtmlGenerator _generator;
        private readonly HtmlEncoder _encoder;

        public AbpPaginationTagHelperService(IHtmlGenerator generator, HtmlEncoder encoder)
        {
            _generator = generator;
            _encoder = encoder;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Model.ShownItemsCount <= 0)
            {
                output.SuppressOutput();
            }

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            var html = new StringBuilder("");

            html.AppendLine(GetOpeningTags(context,output));
            html.AppendLine(GetPreviousButton(context,output));
            html.AppendLine(GetPages(context,output));
            html.AppendLine(GetNextButton(context,output));
            html.AppendLine(GetClosingTags(context,output));

            output.Content.SetHtmlContent(html.ToString());
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
            else
            {
                if (TagHelper.Model.CurrentPage == page.Index)
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
            }

            pageHtml.AppendLine("</li>");

            return pageHtml.ToString();
        }



        protected virtual string GetPreviousButton(TagHelperContext context, TagHelperOutput output)
        {
            var content = "Previous Button";
            var currentPage = TagHelper.Model.CurrentPage == 1
                ? TagHelper.Model.CurrentPage.ToString()
                : (TagHelper.Model.CurrentPage - 1).ToString();
            return
                "<li class=\"page-item " + (TagHelper.Model.CurrentPage == 1 ? "disabled" : "") + "\">\r\n" +
                RenderAnchorTagHelperLinkHtml(context, output, currentPage, content) +
                "                </li>";
        }

        protected virtual string GetNextButton(TagHelperContext context, TagHelperOutput output)
        {
            var content = "Next Button";
            var currentPage = (TagHelper.Model.CurrentPage + 1).ToString();
            return
                "<li class=\"page-item " + (TagHelper.Model.CurrentPage >= TagHelper.Model.TotalPageCount ? "disabled" : "") + "\">\r\n" +
                RenderAnchorTagHelperLinkHtml(context, output, currentPage, content) +
                "                </li>";
        }

        protected virtual string RenderAnchorTagHelperLinkHtml(TagHelperContext context, TagHelperOutput output, string currentPage, string content)
        {
            var anchorTagHelper = new AnchorTagHelper(_generator)
            {
                Page = TagHelper.Model.PageUrl,
                ViewContext = TagHelper.ViewContext
            };

            anchorTagHelper.RouteValues.Add("currentPage", currentPage);
            anchorTagHelper.RouteValues.Add("sort", TagHelper.Model.Sort);

            var attributeList = new TagHelperAttributeList
            {
                new TagHelperAttribute("tabindex", "-1"),
                new TagHelperAttribute("class", "page-link")
            };

            var tagHelperOutput = GetInnerTagHelper(attributeList, context, anchorTagHelper, "a", TagMode.StartTagAndEndTag);

            tagHelperOutput.Content.SetHtmlContent(content);

            var renderedHtml = RenderTagHelperOutput(tagHelperOutput, _encoder);

            return renderedHtml;
        }

        protected virtual string GetOpeningTags(TagHelperContext context, TagHelperOutput output)
        {
            return
                "<div class=\"row mt-3\">\r\n" +
                // "    <div class=\"col-sm-12 col-md-5\">@L[\"PagerInfo\", Model.ShowingFrom, Model.ShowingTo, Model.TotalItemsCount]</div>\r\n" +  <<<<<<<<< No localization for now!
                "    <div class=\"col-sm-12 col-md-5\"> " + TagHelper.Model.ShowingFrom + " " + TagHelper.Model.ShowingTo + " " + TagHelper.Model.TotalItemsCount + " @L[\"PagerInfo\", Model.ShowingFrom, Model.ShowingTo, Model.TotalItemsCount]</div>\r\n" +
                "    <div class=\"col-sm-12 col-md-7\">\r\n" +
                "        <nav aria-label=\"Page navigation\">\r\n" +
                "            <ul class=\"pagination justify-content-end\">";
        }

        protected virtual string GetClosingTags(TagHelperContext context, TagHelperOutput output)
        {
            return
                "            </ul>\r\n" +
                "         </ nav>\r\n" +
                "    </div>\r\n" +
                "</div>";
        }
    }
}