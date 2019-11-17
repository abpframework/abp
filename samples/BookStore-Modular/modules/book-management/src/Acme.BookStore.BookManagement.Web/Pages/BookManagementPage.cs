using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Acme.BookStore.BookManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.BookStore.BookManagement.Web.Pages
{
    /* Inherit your UI Pages from this class. To do that, add this line to your Pages (.cshtml files under the Page folder):
     * @inherits Acme.BookStore.BookManagement.Web.Pages.BookManagementPage
     */
    public abstract class BookManagementPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<BookManagementResource> L { get; set; }
    }
}
