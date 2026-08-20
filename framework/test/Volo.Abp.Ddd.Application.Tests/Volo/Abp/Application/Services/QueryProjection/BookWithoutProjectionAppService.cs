using System;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookWithoutProjectionAppService : CrudAppService<Book, BookDto, Guid>
{
    protected override IQueryProjectionMapper<Book, BookDto> GetProjectionMapper => null;

    protected override IQueryProjectionMapper<Book, BookDto> GetListProjectionMapper => null;

    public BookWithoutProjectionAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {

    }
}
