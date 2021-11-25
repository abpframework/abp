using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Pages;

public interface IPageRepository : IBasicRepository<Page, Guid>
{
    Task<int> GetCountAsync(string filter = null, CancellationToken cancellationToken = default);

    Task<List<Page>> GetListAsync(
        string filter = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default);

    Task<Page> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<Page> FindBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string slug, CancellationToken cancellationToken = default);
}
