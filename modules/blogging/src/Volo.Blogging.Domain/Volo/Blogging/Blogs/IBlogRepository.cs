using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Blogging.Blogs
{
    public interface IBlogRepository : IBasicRepository<Blog, Guid>
    {
        Task<Blog> FindByShortNameAsync(string shortName, CancellationToken cancellationToken = default);
    }
}
