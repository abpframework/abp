#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookWithoutProjectionAppService : CrudAppService<Book, BookDto, Guid>
{
    public const string Marker = "-not-projected";

    protected override IQueryProjector<Book, BookDto>? GetOutputDtoQueryProjector => null;

    protected override IQueryProjector<Book, BookDto>? GetListOutputDtoQueryProjector => null;

    public BookWithoutProjectionAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {

    }

    protected override Task<BookDto> MapToGetOutputDtoAsync(Book entity)
    {
        return Task.FromResult(new BookDto { Id = entity.Id, Name = entity.Name + Marker });
    }

    protected override Task<List<BookDto>> MapToGetListOutputDtosAsync(List<Book> entities)
    {
        return Task.FromResult(entities.ConvertAll(entity => new BookDto { Id = entity.Id, Name = entity.Name + Marker }));
    }
}
