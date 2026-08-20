using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookCustomizedAppService : CrudAppService<Book, BookDto, Guid>
{
    public const string Marker = "-customized";

    public BookCustomizedAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {

    }

    protected override async Task<Book> GetEntityByIdAsync(Guid id)
    {
        var book = await base.GetEntityByIdAsync(id);
        book.Name += Marker;
        return book;
    }

    protected override Task<BookDto> MapToGetOutputDtoAsync(Book entity)
    {
        return Task.FromResult(new BookDto { Id = entity.Id, Name = entity.Name + Marker });
    }
}
