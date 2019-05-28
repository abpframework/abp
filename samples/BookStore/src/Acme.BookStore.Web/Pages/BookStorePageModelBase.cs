using Acme.BookStore.Localization.BookStore;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.BookStore.Pages
{
    public abstract class BookStorePageModelBase : AbpPageModel
    {
        protected BookStorePageModelBase()
        {
            LocalizationResourceType = typeof(BookStoreResource);
        }
    }
}