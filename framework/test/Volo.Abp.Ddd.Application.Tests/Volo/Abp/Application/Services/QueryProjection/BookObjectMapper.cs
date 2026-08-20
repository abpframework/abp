using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookObjectMapper :
    IObjectMapper<Book, BookDto>,
    IObjectMapper<Book, BookLiteDto>,
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
}
