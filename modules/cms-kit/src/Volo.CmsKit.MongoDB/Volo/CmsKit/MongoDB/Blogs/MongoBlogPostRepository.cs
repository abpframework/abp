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

namespace Volo.CmsKit.MongoDB.Blogs;

public class MongoBlogPostRepository : MongoDbRepository<CmsKitMongoDbContext, BlogPost, Guid>, IBlogPostRepository
{
    public MongoBlogPostRepository(IMongoDbContextProvider<CmsKitMongoDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public virtual async Task<BlogPost> GetBySlugAsync(Guid blogId, [NotNull] string slug,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(slug, nameof(slug));

        var token = GetCancellationToken(cancellationToken);

        var blogPost = await GetAsync(x =>
                x.BlogId == blogId &&
                x.Slug.ToLower() == slug,
            cancellationToken: token);

        blogPost.Author = await (await GetMongoQueryableAsync<CmsUser>(token)).FirstOrDefaultAsync(x => x.Id == blogPost.AuthorId, token);

        return blogPost;
    }

    public virtual async Task<int> GetCountAsync(
        string filter = null,
        Guid? blogId = null,
        Guid? authorId = null,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(token))
            .WhereIf<BlogPost, IMongoQueryable<BlogPost>>(!string.IsNullOrWhiteSpace(filter), x => x.Title.Contains(filter) || x.Slug.Contains(filter))
            .WhereIf<BlogPost, IMongoQueryable<BlogPost>>(blogId.HasValue, x => x.BlogId == blogId)
            .WhereIf<BlogPost, IMongoQueryable<BlogPost>>(authorId.HasValue, x => x.AuthorId == authorId)
            .CountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<BlogPost>> GetListAsync(
        string filter = null,
        Guid? blogId = null,
        Guid? authorId = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);
        var dbContext = await GetDbContextAsync(token);
        var blogPostQueryable = await GetQueryableAsync();

        var usersQueryable = dbContext.Collection<CmsUser>().AsQueryable();

        var queryable = blogPostQueryable
            .WhereIf(blogId.HasValue, x => x.BlogId == blogId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Title.Contains(filter) || x.Slug.Contains(filter))
            .WhereIf(authorId.HasValue, x => x.AuthorId == authorId);

        queryable = queryable.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(BlogPost.CreationTime)} desc" : sorting);

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

    public virtual async Task<bool> SlugExistsAsync(Guid blogId, [NotNull] string slug,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(slug, nameof(slug));

        var token = GetCancellationToken(cancellationToken);
        var queryable = await GetMongoQueryableAsync(token);
        return await queryable.AnyAsync(x => x.BlogId == blogId && x.Slug.ToLower() == slug, token);
    }

    public async Task<List<CmsUser>> GetAuthorsHasBlogPosts(CancellationToken cancellationToken = default)
    {
        var blogPostQueryable = (await GetQueryableAsync());

        var usersQueryable = (await GetDbContextAsync()).Collection<CmsUser>().AsQueryable();

        var queryable = blogPostQueryable
                        .Join(
                            usersQueryable,
                            o => o.AuthorId,
                            i => i.Id,
                            (blogPost, user) => new { blogPost, user })
                        .Select(s => s.user)
                        .Distinct();

        return await AsyncExecuter.ToListAsync(queryable, GetCancellationToken(cancellationToken));
    }
}
