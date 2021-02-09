using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogPostRepository : IBasicRepository<BlogPost, Guid>
    {
        Task<bool> SlugExistsAsync(Guid blogId, string slug, CancellationToken cancellationToken = default);

        Task<BlogPost> GetBySlugAsync(Guid blogId, string slug, CancellationToken cancellationToken = default);

        Task<List<BlogPost>> GetPagedListAsync(Guid blogId, int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(Guid blogId, CancellationToken cancellationToken = default);
    }
}
