using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Markdig;
using Volo.Abp.DependencyInjection;
using Volo.Blogging.Localization;

namespace Volo.Blogging.Pages.Blog
{
    public class BloggingPageHelper : ITransientDependency
    {
        public IHtmlLocalizer<BloggingResource> L { get; set; }

        public const string DefaultTitle = "Blog";

        public const int MaxShortContentLength = 200;

        public string GetTitle(string title = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return DefaultTitle;
            }

            return title;
        }

        public string GetShortContent(string content)
        {
            var html = RenderMarkdownToHtmlAsString(content);
            var plainText = Regex.Replace(html, "<[^>]*>", "");

            if (string.IsNullOrWhiteSpace(plainText))
            {
                return "";
            }

            var shortContent = new StringBuilder();
            var lines = plainText.Split(Environment.NewLine).Where(s => !string.IsNullOrWhiteSpace(s));

            foreach (var line in lines)
            {
                if (shortContent.Length < MaxShortContentLength)
                {
                    shortContent.Append($" {line}");
                }

                if(shortContent.Length >= MaxShortContentLength)
                {
                    return shortContent.ToString().Substring(0, MaxShortContentLength) + "...";
                }
            }

            return shortContent.ToString();
        }

        public IHtmlContent RenderMarkdownToHtml(string content)
        {
            if(content.IsNullOrWhiteSpace())
            {
                return new HtmlString("");
            }

            var html = RenderMarkdownToHtmlAsString(content);

            html = ReplaceCodeBlocksLanguage(
                html,
                "language-C#",
                "language-csharp"
            );

            return new HtmlString(html);
        }

        protected string ReplaceCodeBlocksLanguage(string content, string currentLanguage, string newLanguage)
        {
            return Regex.Replace(content, "<code class=\"" + currentLanguage + "\">", "<code class=\"" + newLanguage + "\">", RegexOptions.IgnoreCase);
        }

        public string RenderMarkdownToHtmlAsString(string content)
        {
            if (content.IsNullOrWhiteSpace())
            {
                return "";
            }

            return Markdig.Markdown.ToHtml(Encoding.UTF8.GetString(Encoding.Default.GetBytes(content)),
                new MarkdownPipelineBuilder()
                    .UseAutoLinks()
                    .UseBootstrap()
                    .UseGridTables()
                    .UsePipeTables()
                    .Build());
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
