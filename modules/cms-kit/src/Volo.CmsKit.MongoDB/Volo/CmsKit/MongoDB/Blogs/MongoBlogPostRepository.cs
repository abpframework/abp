using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.MongoDB.Blogs
{
    public class MongoBlogPostRepository : MongoDbRepository<CmsKitMongoDbContext, BlogPost, Guid>, IBlogPostRepository
    {
        public MongoBlogPostRepository(IMongoDbContextProvider<CmsKitMongoDbContext> dbContextProvider) : base(
            dbContextProvider)
        {
        }

        public Task<BlogPost> GetBySlugAsync(Guid blogId, [NotNull] string slug,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(slug, nameof(slug));

            return GetAsync(x =>
                    x.BlogId == blogId &&
                    x.Slug.ToLower() == slug,
                includeDetails: true,
                cancellationToken: GetCancellationToken(cancellationToken));
        }

        public async Task<int> GetCountAsync(Guid blogId, CancellationToken cancellationToken = default)
        {
            return await AsyncExecuter.CountAsync(
                await WithDetailsAsync(),
                x => x.BlogId == blogId,
                GetCancellationToken(cancellationToken));
        }

        public async Task<List<BlogPost>> GetPagedListAsync(Guid blogId, int skipCount, int maxResultCount,
            string sorting, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var token = GetCancellationToken(cancellationToken);
            var dbContext = await GetDbContextAsync(token);
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

            return await AsyncExecuter.ToListAsync(queryable, token);
        }

        public async Task<bool> SlugExistsAsync(Guid blogId, [NotNull] string slug,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(slug, nameof(slug));

            var token = GetCancellationToken(cancellationToken);
            var queryable = await GetMongoQueryableAsync(token);
            return await queryable.AnyAsync(x => x.BlogId == blogId && x.Slug.ToLower() == slug, token);
        }
    }
}