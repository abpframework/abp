using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookDetailAppService :
    ReadOnlyAppService<Book, BookDetailDto, BookDto, Guid, PagedAndSortedResultRequestDto>
{
    public BookDetailAppService(IReadOnlyRepository<Book, Guid> repository)
        : base(repository)
    {

    }
}
