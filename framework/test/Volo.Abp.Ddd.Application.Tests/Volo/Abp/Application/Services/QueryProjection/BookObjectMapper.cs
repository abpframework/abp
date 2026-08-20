using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookObjectMapper :
    IObjectMapper<Book, BookDto>,
    IObjectMapper<Book, BookLiteDto>,
    IObjectMapper<BookDto, Book>,
    ITransientDependency
{
    public const string Marker = "-mapped";

    public BookDto Map(Book source)
    {
        return new BookDto { Id = source.Id, Name = source.Name + Marker };
    }

    public BookDto Map(Book source, BookDto destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name + Marker;
        return destination;
    }

    BookLiteDto IObjectMapper<Book, BookLiteDto>.Map(Book source)
    {
        return new BookLiteDto { Id = source.Id, Name = source.Name + Marker };
    }

    BookLiteDto IObjectMapper<Book, BookLiteDto>.Map(Book source, BookLiteDto destination)
    {
        destination.Id = source.Id;
        destination.Name = source.Name + Marker;
        return destination;
    }

    Book IObjectMapper<BookDto, Book>.Map(BookDto source)
    {
        return new Book(source.Id, source.Name, 0);
    }

    Book IObjectMapper<BookDto, Book>.Map(BookDto source, Book destination)
    {
        destination.Name = source.Name;
        return destination;
    }
}
