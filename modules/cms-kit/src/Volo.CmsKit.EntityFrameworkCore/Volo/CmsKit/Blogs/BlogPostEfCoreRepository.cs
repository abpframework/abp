using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostEfCoreRepository : EfCoreRepository<CmsKitDbContext, BlogPost, Guid>, IBlogPostRepository
    {
        public BlogPostEfCoreRepository(IDbContextProvider<CmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<BlogPost> GetByUrlSlugAsync(string urlSlug)
        {
            return GetAsync(x => x.UrlSlug.ToLower() == urlSlug);
        }

        public async Task<bool> SlugExistsAsync(string slug)
        {
            var dbSet = await GetDbSetAsync();

            return await dbSet.AnyAsync(x => x.UrlSlug.ToLower() == slug);
        }
    }
}
