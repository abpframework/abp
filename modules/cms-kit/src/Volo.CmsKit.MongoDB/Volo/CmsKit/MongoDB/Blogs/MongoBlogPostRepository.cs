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
using Volo.Abp.Domain.Entities;
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
        BlogPostStatus? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(cancellationToken))
            .WhereIf<BlogPost, IMongoQueryable<BlogPost>>(!string.IsNullOrWhiteSpace(filter), x => x.Title.Contains(filter) || x.Slug.Contains(filter))
            .WhereIf<BlogPost, IMongoQueryable<BlogPost>>(blogId.HasValue, x => x.BlogId == blogId)
            .WhereIf<BlogPost, IMongoQueryable<BlogPost>>(authorId.HasValue, x => x.AuthorId == authorId)
            .WhereIf<BlogPost, IMongoQueryable<BlogPost>>(statusFilter.HasValue, x => x.Status == statusFilter)
            .CountAsync(cancellationToken);
    }

    public virtual async Task<List<BlogPost>> GetListAsync(
        string filter = null,
        Guid? blogId = null,
        Guid? authorId = null,
        BlogPostStatus? statusFilter = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string sorting = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var dbContext = await GetDbContextAsync(cancellationToken);
        var blogPostQueryable = await GetQueryableAsync();

        var usersQueryable = dbContext.Collection<CmsUser>().AsQueryable();

        var queryable = blogPostQueryable
            .WhereIf(blogId.HasValue, x => x.BlogId == blogId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Title.Contains(filter) || x.Slug.Contains(filter))
            .WhereIf(authorId.HasValue, x => x.AuthorId == authorId)
            .WhereIf(statusFilter.HasValue, x => x.Status == statusFilter);

        queryable = queryable.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(BlogPost.CreationTime)} desc" : sorting);

        var combinedQueryable = queryable
                                .Join(
                                    usersQueryable,
                                    o => o.AuthorId,
                                    i => i.Id,
                                    (blogPost, user) => new { blogPost, user })
                                .Skip(skipCount)
                                .Take(maxResultCount);

        var combinedResult = await AsyncExecuter.ToListAsync(combinedQueryable, cancellationToken);

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

        cancellationToken = GetCancellationToken(cancellationToken);
        var queryable = await GetMongoQueryableAsync(cancellationToken);
        return await queryable.AnyAsync(x => x.BlogId == blogId && x.Slug.ToLower() == slug, cancellationToken);
    }

    public async Task<List<CmsUser>> GetAuthorsHasBlogPostsAsync(int skipCount, int maxResultCount, string sorting, string filter, CancellationToken cancellationToken = default)
    {
        var queryable = (await CreateAuthorsQueryableAsync())
                        .Skip(skipCount)
                        .Take(maxResultCount)
                        .OrderBy(sorting.IsNullOrEmpty() ? nameof(CmsUser.UserName) : sorting)
                        .WhereIf(!filter.IsNullOrEmpty(), x => x.UserName.Contains(filter.ToLower()));

        return await AsyncExecuter.ToListAsync(queryable, GetCancellationToken(cancellationToken));
    }

    public async Task<int> GetAuthorsHasBlogPostsCountAsync(string filter, CancellationToken cancellationToken = default)
    {
        return await AsyncExecuter.CountAsync(
            (await CreateAuthorsQueryableAsync())
                .WhereIf(!filter.IsNullOrEmpty(), x => x.UserName.Contains(filter.ToLower())));
    }

    public async Task<CmsUser> GetAuthorHasBlogPostAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await AsyncExecuter.FirstOrDefaultAsync(await CreateAuthorsQueryableAsync(), x => x.Id == id)
            ?? throw new EntityNotFoundException(typeof(CmsUser), id);
    }

    private async Task<IQueryable<CmsUser>> CreateAuthorsQueryableAsync()
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        
        var blogPostQueryable = (await GetQueryableAsync())
            .Where(x => x.Status == BlogPostStatus.Published);

        var usersQueryable = (await GetDbContextAsync(cancellationToken)).Collection<CmsUser>().AsQueryable();

        return blogPostQueryable
                        .Join(
                            usersQueryable,
                            o => o.AuthorId,
                            i => i.Id,
                            (blogPost, user) => new { blogPost, user })
                        .Select(s => s.user)
                        .Distinct();

        return await AsyncExecuter.ToListAsync(queryable, cancellationToken);
    }
}
