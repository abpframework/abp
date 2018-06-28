using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Acme.BookStore.Localization.BookStore;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.BookStore.Pages
{
    public abstract class BookStorePageBase : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<BookStoreResource> L { get; set; }
    }
}
