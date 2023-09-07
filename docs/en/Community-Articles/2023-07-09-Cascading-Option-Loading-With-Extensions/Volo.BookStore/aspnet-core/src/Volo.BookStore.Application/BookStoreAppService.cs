using System;
using System.Collections.Generic;
using System.Text;
using Volo.BookStore.Localization;
using Volo.Abp.Application.Services;

namespace Volo.BookStore;

/* Inherit your application services from this class.
 */
public abstract class BookStoreAppService : ApplicationService
{
    protected BookStoreAppService()
    {
        LocalizationResource = typeof(BookStoreResource);
    }
}
