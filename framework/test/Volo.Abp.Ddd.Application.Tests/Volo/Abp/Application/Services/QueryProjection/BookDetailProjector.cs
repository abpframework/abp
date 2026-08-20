using System.Linq;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookDetailProjector : IQueryProjector<Book, BookDetailDto>
{
    public const string Marker = "-detail";

    public IQueryable<BookDetailDto> ProjectTo(IQueryable<Book> source)
    {
        return source.Select(book => new BookDetailDto
        {
            Id = book.Id,
            Name = book.Name + Marker
        });
    }
}
