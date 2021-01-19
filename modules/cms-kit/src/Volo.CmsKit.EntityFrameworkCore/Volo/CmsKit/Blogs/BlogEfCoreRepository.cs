using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Blogs
{
    public class BlogEfCoreRepository : EfCoreRepository<CmsKitDbContext, Blog, Guid>, IBlogRepository
    {
        public BlogEfCoreRepository(IDbContextProvider<CmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
