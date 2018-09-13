using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Blogging.Blogs
{
    public interface IBlogRepository : IBasicRepository<Blog, Guid>
    {
        Task<Blog> FindByShortNameAsync(string shortName);

        Task<List<Blog>> GetListAsync(string sorting, int maxResultCount, int skipCount);

        Task<int> GetTotalBlogCount();
    }
}
