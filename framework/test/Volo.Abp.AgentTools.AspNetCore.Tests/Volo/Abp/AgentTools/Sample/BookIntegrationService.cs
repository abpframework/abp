using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.AgentTools.Sample;

/// <summary>Integration service under the same remote service; exposed over HTTP (ExposeIntegrationServices) but never a tool.</summary>
[IntegrationService]
public interface IBookIntegrationService : IApplicationService
{
    Task<List<BookDto>> GetAllAsync();
}

[IntegrationService]
public class BookIntegrationService : ApplicationService, IBookIntegrationService
{
    private readonly IRepository<Book, Guid> _books;

    public BookIntegrationService(IRepository<Book, Guid> books)
    {
        _books = books;
    }

    public async Task<List<BookDto>> GetAllAsync()
    {
        return (await _books.GetListAsync()).Select(b => new BookDto { Id = b.Id, Name = b.Name }).ToList();
    }
}
