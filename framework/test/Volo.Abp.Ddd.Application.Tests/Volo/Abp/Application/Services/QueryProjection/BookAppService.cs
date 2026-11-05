using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookAppService : CrudAppService<Book, BookDto, Guid>
{
    public BookAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {

    }
}
