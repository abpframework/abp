using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.MongoDB.Blogs
{
    public class MongoBlogPostRepository : MongoDbRepository<CmsKitMongoDbContext, BlogPost, Guid>, IBlogPostRepository
    {
        public MongoBlogPostRepository(IMongoDbContextProvider<CmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<BlogPost> GetBySlugAsync(Guid blogId, string slug, CancellationToken cancellationToken = default)
        {
            return GetAsync(x => 
                            x.BlogId == blogId && 
                            x.Slug.ToLower() == slug,
                        includeDetails: true,
                        cancellationToken: cancellationToken);
        }

        public async Task<int> GetCountAsync(Guid blogId, CancellationToken cancellationToken = default)
        {
            return await AsyncExecuter.CountAsync(
                            await WithDetailsAsync(),
                            x => x.BlogId == blogId,
                            cancellationToken);
        }

        public async Task<List<BlogPost>> GetPagedListAsync(Guid blogId, int skipCount, int maxResultCount, string sorting, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync(cancellationToken);
            var blogPostQueryable = await WithDetailsAsync();

            var queryable = blogPostQueryable
                          .Where(x => x.BlogId == blogId);

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
            var queryable = await GetMongoQueryableAsync();

            return await queryable.AnyAsync(x => x.BlogId == blogId && x.Slug.ToLower() == slug, cancellationToken);
        }
    }
}
