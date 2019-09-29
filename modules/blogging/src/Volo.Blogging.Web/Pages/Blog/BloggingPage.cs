using System;
using System.Text;
using CommonMark;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Localization;

namespace Volo.Blogging.Pages.Blog
{
    public abstract class BloggingPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<BloggingResource> L { get; set; }

        public const string DefaultTitle = "Blog";

        public const int MaxShortContentLength = 128;

        public string GetTitle(string title = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return DefaultTitle;
            }

            return title;
        }

        public string GetShortContent(string content) //TODO: This should be moved to its own place!
        {
            var openingTag = "<p>";
            var closingTag = "</p>";

            var html = RenderMarkdownToString(content);
            if (string.IsNullOrWhiteSpace(html))
            {
                return "";
            }
            var splittedHtml = html.Split(closingTag);

            if (splittedHtml.Length < 1)
            {
                return "";
            }

            var firstHtmlPart = splittedHtml[0];
            var paragraphStartIndex = firstHtmlPart.IndexOf(openingTag, StringComparison.Ordinal) + openingTag.Length;

            if (firstHtmlPart.Length - paragraphStartIndex <= MaxShortContentLength)
            {
                return firstHtmlPart.Substring(paragraphStartIndex);
            }

            return firstHtmlPart.Substring(paragraphStartIndex, MaxShortContentLength) + "...";
        }

        public IHtmlContent RenderMarkdownToHtml(string content)
        {
            if(content.IsNullOrWhiteSpace())
            {
                return new HtmlString("");
            }
            
            byte[] bytes = Encoding.Default.GetBytes(content);
            var utf8Content = Encoding.UTF8.GetString(bytes);

            var html = CommonMarkConverter.Convert(utf8Content);

            return new HtmlString(html);
        }

        public string RenderMarkdownToString(string content)
        {
            if (content.IsNullOrWhiteSpace())
            {
                return "";
            }

            byte[] bytes = Encoding.Default.GetBytes(content);
            var utf8Content = Encoding.UTF8.GetString(bytes);

            return CommonMarkConverter.Convert(utf8Content);
        }

        public LocalizedHtmlString ConvertDatetimeToTimeAgo(DateTime dt)
        {
            var timeDiff = DateTime.Now - dt;

            var diffInDays = (int) timeDiff.TotalDays;

            if (diffInDays >= 365)
            {
                return  L["YearsAgo", diffInDays / 365];
            }
            if (diffInDays >= 30)
            {
                return L["MonthsAgo", diffInDays / 30];
            }
            if (diffInDays >= 7)
            {
                return L["WeeksAgo", diffInDays / 7];
            }
            if (diffInDays >= 1)
            {
                return L["DaysAgo", diffInDays];
            }

            var diffInSeconds = (int) timeDiff.TotalSeconds;

            if (diffInSeconds >= 3600)
            {
                return L["HoursAgo", diffInSeconds / 3600];
            }
            if (diffInSeconds >= 60)
            {
                return L["MinutesAgo", diffInSeconds / 60];
            }
            if (diffInSeconds >= 1)
            {
                return  L["SecondsAgo", diffInSeconds];
            }

            return L["Now"];
        }
    }
}
