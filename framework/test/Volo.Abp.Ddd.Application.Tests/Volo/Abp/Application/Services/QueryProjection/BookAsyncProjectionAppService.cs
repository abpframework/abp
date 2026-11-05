#nullable enable
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookAsyncProjectionAppService : CrudAppService<Book, BookDto, Guid>
{
    public const string Marker = "-async";

    private readonly IBookNameSuffixProvider _suffixProvider;

    public BookAsyncProjectionAppService(
        IRepository<Book, Guid> repository,
        IBookNameSuffixProvider suffixProvider)
        : base(repository)
    {
        _suffixProvider = suffixProvider;
    }

    protected override async Task<IQueryable<BookDto>?> CreateGetOutputDtoQueryOrNullAsync(Guid id)
    {
        var query = await Repository.GetQueryableAsync();

        return await ProjectAsync(query.Where(book => book.Id == id));
    }

    protected override async Task<IQueryable<BookDto>?> CreateGetListOutputDtoQueryOrNullAsync(IQueryable<Book> query)
    {
        return await ProjectAsync(query);
    }

    private async Task<IQueryable<BookDto>> ProjectAsync(IQueryable<Book> query)
    {
        var suffix = await _suffixProvider.GetAsync();

        return query.Select(book => new BookDto
        {
            Id = book.Id,
            Name = book.Name + suffix
        });
    }
}
