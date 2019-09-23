using Acme.BookStore.BookManagement.Localization;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.BookManagement
{
    public abstract class BookManagementAppService : ApplicationService
    {
        protected BookManagementAppService()
        {
            LocalizationResource = typeof(BookManagementResource);
        }
    }
}
