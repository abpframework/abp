using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogRepository : IBasicRepository<Blog, Guid>
    {
        public Task<Blog> GetByUrlSlugAsync(string urlSlug);
        Task<bool> ExistsAsync(Guid blogId);
    }
}
