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
    }
}
