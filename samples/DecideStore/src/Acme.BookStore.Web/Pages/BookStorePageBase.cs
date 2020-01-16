using Acme.BookStore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.BookStore.Web.Pages
{
    public abstract class BookStorePageBase : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<BookStoreResource> L { get; set; }
    }
}
