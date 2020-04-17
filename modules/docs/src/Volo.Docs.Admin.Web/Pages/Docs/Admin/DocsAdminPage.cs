using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Docs.Localization;

namespace Volo.Docs.Admin.Pages.Docs.Admin
{
    public abstract class DocsAdminPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<DocsResource> L { get; set; }
    }
}
