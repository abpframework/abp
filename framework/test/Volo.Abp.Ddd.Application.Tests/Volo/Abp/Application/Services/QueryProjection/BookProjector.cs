using System.Linq;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookProjector : IQueryableMapper<Book, BookDto>
{
    public const string Marker = "-projected";

    public IQueryable<BookDto> ProjectTo(IQueryable<Book> source)
    {
        return source.Select(book => new BookDto
        {
            Id = book.Id,
            Name = book.Name + Marker
        });
    }
}
