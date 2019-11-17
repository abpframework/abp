using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Acme.BookStore.BookManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.BookStore.BookManagement.Pages
{
    public abstract class BookManagementPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<BookManagementResource> L { get; set; }
    }
}
