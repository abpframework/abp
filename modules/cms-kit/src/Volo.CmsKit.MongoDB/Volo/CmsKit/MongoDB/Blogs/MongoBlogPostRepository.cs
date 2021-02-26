using JetBrains.Annotations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<BlogPost> GetBySlugAsync(Guid blogId, [NotNull] string slug,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrEmpty(slug, nameof(slug));

            var blogPost = await GetAsync(x =>
                    x.BlogId == blogId &&
                    x.Slug.ToLower() == slug,
                cancellationToken: GetCancellationToken(cancellationToken));

            var dbContext = await GetDbContextAsync();

            blogPost.Author = await dbContext.Collection<CmsUser>().AsQueryable().FirstOrDefaultAsync(x => x.Id == blogPost.AuthorId);

            return blogPost;
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
            var blogPostQueryable = dbContext.Collection<BlogPost>().AsQueryable();
            var usersQueryable = dbContext.Collection<CmsUser>().AsQueryable();

            IQueryable<BlogPost> queryable = blogPostQueryable
                .Where(x => x.BlogId == blogId);

            if (!sorting.IsNullOrWhiteSpace())
            {
                queryable = queryable.OrderBy(sorting);
            }

            var combinedQueryable = queryable
                                    .Join(
                                        usersQueryable,
                                        o => o.AuthorId,
                                        i => i.Id,
                                        (blogPost, user) => new { blogPost, user })
                                    .Skip(skipCount)
                                    .Take(maxResultCount);

            var combinedResult = await AsyncExecuter.ToListAsync(combinedQueryable, GetCancellationToken(cancellationToken));

            return combinedResult.Select(s =>
                                        {
                                            s.blogPost.Author = s.user;
                                            return s.blogPost;
                                        }).ToList();
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