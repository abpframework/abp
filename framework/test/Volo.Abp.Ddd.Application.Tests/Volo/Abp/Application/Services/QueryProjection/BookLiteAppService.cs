using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookLiteAppService : CrudAppService<Book, BookLiteDto, Guid>
{
    public BookLiteAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {

    }
}
