using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Blogs;

public interface IBlogPostRepository : IBasicRepository<BlogPost, Guid>
{
    Task<int> GetCountAsync(
        string filter = null,
        Guid? blogId = null,
        CancellationToken cancellationToken = default);

    Task<List<BlogPost>> GetListAsync(
        string filter = null,
        Guid? blogId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default);

    Task<bool> SlugExistsAsync(Guid blogId, string slug, CancellationToken cancellationToken = default);

    Task<BlogPost> GetBySlugAsync(Guid blogId, string slug, CancellationToken cancellationToken = default);
}
