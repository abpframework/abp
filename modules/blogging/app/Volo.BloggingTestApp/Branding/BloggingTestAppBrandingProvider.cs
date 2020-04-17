using Microsoft.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;
using Volo.Blogging.Localization;

namespace Volo.BloggingTestApp.Branding
{
    [Dependency(ReplaceServices = true)]
    public class BloggingTestAppBrandingProvider : DefaultBrandingProvider
    {
        public IHtmlLocalizer<BloggingResource> L { get; set; }
        public BloggingTestAppBrandingProvider(IHtmlLocalizer<BloggingResource> localizer)
        {
            L = localizer;
        }
        public override string AppName => L["Blogs"].Value;
    }
}
