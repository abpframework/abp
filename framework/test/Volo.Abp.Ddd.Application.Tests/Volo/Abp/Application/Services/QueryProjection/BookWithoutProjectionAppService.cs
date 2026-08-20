using System;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookWithoutProjectionAppService : CrudAppService<Book, BookDto, Guid>
{
    protected override IQueryableMapper<Book, BookDto> GetQueryableMapper => null;

    protected override IQueryableMapper<Book, BookDto> GetListQueryableMapper => null;

    public BookWithoutProjectionAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {

    }
}
