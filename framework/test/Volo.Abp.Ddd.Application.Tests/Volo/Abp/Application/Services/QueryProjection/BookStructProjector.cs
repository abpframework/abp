using System.Linq;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookStructProjector : IQueryProjector<Book, BookStructDto>
{
    public IQueryable<BookStructDto> ProjectTo(IQueryable<Book> source)
    {
        return source.Select(book => new BookStructDto
        {
            Id = book.Id,
            Name = book.Name
        });
    }
}
