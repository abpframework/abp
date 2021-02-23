using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogRepository : IBasicRepository<Blog, Guid>
    {
        public Task<Blog> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        
        Task<bool> ExistsAsync(Guid blogId, CancellationToken cancellationToken = default);
        
        Task<bool> HasPostsAsync(Guid blogId, CancellationToken cancellationToken = default);
    }
}
