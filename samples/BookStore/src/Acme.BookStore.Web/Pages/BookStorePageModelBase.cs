using Microsoft.AspNetCore.Mvc.Localization;
using Acme.BookStore.Localization.BookStore;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.BookStore.Pages
{
    public abstract class BookStorePageModelBase : AbpPageModel
    {
        public IHtmlLocalizer<BookStoreResource> L { get; set; }
    }
}