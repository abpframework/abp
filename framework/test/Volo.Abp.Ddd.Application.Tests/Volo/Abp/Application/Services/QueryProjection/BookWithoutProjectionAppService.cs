#nullable enable
using System;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookWithoutProjectionAppService : CrudAppService<Book, BookDto, Guid>
{
    protected override IQueryProjector<Book, BookDto>? GetOutputDtoQueryProjector => null;

    protected override IQueryProjector<Book, BookDto>? GetListOutputDtoQueryProjector => null;

    public BookWithoutProjectionAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {

    }
}
