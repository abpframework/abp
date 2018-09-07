using System;
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

        public string GetTitle(string title = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return DefaultTitle;
            }

            return title;
        }

        public string ConvertDatetimeToTimeAgo(DateTime dt)
        {
            var timeDiff = DateTime.Now - dt;

            var diffInDays = (int) timeDiff.TotalDays;

            if (diffInDays >= 365)
            {
                return diffInDays / 365 + L["YearsAgo"].Value;
            }
            if (diffInDays >= 30)
            {
                return diffInDays / 30 + L["MonthsAgo"].Value;
            }
            if (diffInDays >= 7)
            {
                return diffInDays / 7 + L["WeeksAgo"].Value;
            }
            if (diffInDays >= 1)
            {
                return diffInDays + L["DaysAgo"].Value;
            }

            var diffInSeconds = (int) timeDiff.TotalSeconds;

            if (diffInSeconds >= 3600)
            {
                return diffInSeconds / 3600 + L["HoursAgo"].Value;
            }
            if (diffInSeconds >= 60)
            {
                return diffInSeconds / 60 + L["MinutesAgo"].Value;
            }
            if (diffInSeconds >= 1)
            {
                return diffInSeconds + L["SecondsAgo"].Value;
            }

            return L["Now"].Value;
        }
    }
}
