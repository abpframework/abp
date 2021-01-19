using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.Blogs
{
    public interface IBlogRepository : IRepository<Blog, Guid>
    {
    }
}
