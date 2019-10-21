using Acme.BookStore.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.BookStore.Web.Pages
{
    public abstract class BookStorePageModelBase : AbpPageModel
    {
        protected BookStorePageModelBase()
        {
            LocalizationResourceType = typeof(BookStoreResource);
        }
    }
}