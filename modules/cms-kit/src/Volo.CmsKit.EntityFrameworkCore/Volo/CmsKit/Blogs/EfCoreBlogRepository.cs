using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Blogs
{
    public class EfCoreBlogRepository : EfCoreRepository<ICmsKitDbContext, Blog, Guid>, IBlogRepository
    {
        public EfCoreBlogRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<Blog> GetByUrlSlugAsync(string urlSlug)
        {
            return GetAsync(x => x.UrlSlug == urlSlug);
        }
    }
}
