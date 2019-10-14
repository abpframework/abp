using Acme.BookStore.BookManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.BookStore.BookManagement.Pages
{
    public abstract class BookManagementPageModel : AbpPageModel
    {
        protected BookManagementPageModel()
        {
            LocalizationResourceType = typeof(BookManagementResource);
        }
    }
}