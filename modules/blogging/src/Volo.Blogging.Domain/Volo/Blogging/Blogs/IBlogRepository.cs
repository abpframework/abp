using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Blogging.Blogs
{
    public interface IBlogRepository : IBasicRepository<Blog, Guid>
    {
        Task<Blog> FindByShortNameAsync(string shortName);
    }
}
