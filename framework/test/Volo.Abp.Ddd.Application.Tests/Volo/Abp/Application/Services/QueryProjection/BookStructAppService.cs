using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookStructAppService : ReadOnlyAppService<Book, BookStructDto, Guid>
{
    public BookStructAppService(IReadOnlyRepository<Book, Guid> repository)
        : base(repository)
    {

    }
}
