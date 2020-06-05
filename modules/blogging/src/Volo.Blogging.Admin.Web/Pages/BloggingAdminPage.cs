using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Localization;

namespace Volo.Blogging.Admin.Pages.Blogging
{
        public abstract class BloggingAdminPage : AbpPage
        {
            [RazorInject]
            public IHtmlLocalizer<BloggingResource> L { get; set; }

            public const string DefaultTitle = "Blogging";

            public const int MaxShortContentLength = 200;
    }
}
