using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookReadOnlyAppService : ReadOnlyAppService<Book, BookDto, Guid>
{
    public BookReadOnlyAppService(IReadOnlyRepository<Book, Guid> repository)
        : base(repository)
    {

    }
}
