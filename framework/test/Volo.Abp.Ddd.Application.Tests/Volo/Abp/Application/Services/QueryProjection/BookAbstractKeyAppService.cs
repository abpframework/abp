using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookAbstractKeyAppService : AbstractKeyReadOnlyAppService<Book, BookDto, Guid>
{
    public BookAbstractKeyAppService(IReadOnlyRepository<Book> repository)
        : base(repository)
    {

    }

    protected override async Task<Book> GetEntityByIdAsync(Guid id)
    {
        var query = await ReadOnlyRepository.GetQueryableAsync();

        return await AsyncExecuter.FirstAsync(query, book => book.Id == id);
    }

    protected override IQueryable<Book> ApplyDefaultSorting(IQueryable<Book> query)
    {
        return query.OrderBy(book => book.Id);
    }
}
