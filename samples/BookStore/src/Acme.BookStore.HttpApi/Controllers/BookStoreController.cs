using Acme.BookStore.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Acme.BookStore.Controllers
{
    public abstract class BookStoreController : AbpController
    {
        protected BookStoreController()
        {
            LocalizationResource = typeof(BookStoreResource);
        }
    }
}