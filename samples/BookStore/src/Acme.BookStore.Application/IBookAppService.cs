using System;
using Volo.Abp.Application.Services;

namespace Acme.BookStore
{
    public interface IBookAppService : IAsyncCrudAppService<BookDto, Guid>
    {

    }
}
