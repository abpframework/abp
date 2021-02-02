using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.MongoDB.Blogs
{
    public class MongoBlogPostRepository : MongoDbRepository<CmsKitMongoDbContext, BlogPost, Guid>, IBlogPostRepository
    {
        public MongoBlogPostRepository(IMongoDbContextProvider<CmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<BlogPost> GetByUrlSlugAsync(Guid blogId, string urlSlug, CancellationToken cancellationToken = default)
        {
            return GetAsync(x => x.BlogId == blogId && x.UrlSlug.ToLower() == urlSlug, cancellationToken: cancellationToken);
        }

        public async Task<List<BlogPost>> GetPagedListAsync(Guid blogId, int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync(cancellationToken);
            var blogPostQueryable = await GetQueryableAsync();

            var queryable = blogPostQueryable
                          .Where(x => x.BlogId == blogId)
                          //.Join(dbContext.CmsUsers, outerKeySelector: x => x.CreatorId, innerKeySelector: x => x.Id, resultSelector: s => s)
                          ;

            if (!sorting.IsNullOrWhiteSpace())
            {
                queryable = queryable.OrderBy(sorting);
            }

            queryable = queryable
                          .Skip(skipCount)
                          .Take(maxResultCount);

            return await AsyncExecuter.ToListAsync(queryable, cancellationToken);
        }

        public async Task<bool> SlugExistsAsync(Guid blogId, string slug, CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();

            return await AsyncExecuter.AnyAsync(queryable, x => x.BlogId == blogId && x.UrlSlug.ToLower() == slug, cancellationToken);
        }
    }
}
