using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Blogs;

public class EfCoreBlogPostRepository : EfCoreRepository<CmsKitDbContext, BlogPost, Guid>, IBlogPostRepository
{
    public EfCoreBlogPostRepository(IDbContextProvider<CmsKitDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<BlogPost> GetBySlugAsync(
        Guid blogId,
        [NotNull] string slug,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(slug, nameof(slug));

        var blogPost = await GetAsync(
                                x => x.BlogId == blogId && x.Slug.ToLower() == slug,
                                cancellationToken: GetCancellationToken(cancellationToken));

        blogPost.Author = await (await GetDbContextAsync())
                            .Set<CmsUser>()
                            .FirstOrDefaultAsync(x => x.Id == blogPost.AuthorId, GetCancellationToken(cancellationToken));

        return blogPost;
    }

    public virtual async Task<int> GetCountAsync(
        string filter = null,
        Guid? blogId = null,
        Guid? authorId = null,
        BlogPostStatus? statusFilter = null,
        CancellationToken cancellationToken = default)
    {
        var queryable = (await GetDbSetAsync())
            .WhereIf(blogId.HasValue, x => x.BlogId == blogId)
            .WhereIf(authorId.HasValue, x => x.AuthorId == authorId)
            .WhereIf(statusFilter.HasValue, x => x.Status == statusFilter)
            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Title.Contains(filter) || x.Slug.Contains(filter));

        var count = await queryable.CountAsync(GetCancellationToken(cancellationToken));
        return count;
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
        var dbContext = await GetDbContextAsync();
        var blogPostsDbSet = dbContext.Set<BlogPost>();
        var usersDbSet = dbContext.Set<CmsUser>();

        var queryable = blogPostsDbSet
            .WhereIf(blogId.HasValue, x => x.BlogId == blogId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter), x => x.Title.Contains(filter) || x.Slug.Contains(filter))
            .WhereIf(authorId.HasValue, x => x.AuthorId == authorId)
            .WhereIf(statusFilter.HasValue, x => x.Status == statusFilter);

        queryable = queryable.OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(BlogPost.CreationTime)} desc" : sorting);

        var combinedResult = await queryable
            .Join(
                usersDbSet,
                o => o.AuthorId,
                i => i.Id,
                (blogPost, user) => new { blogPost, user })
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));

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

        return await (await GetDbSetAsync()).AnyAsync(x => x.BlogId == blogId && x.Slug.ToLower() == slug,
            GetCancellationToken(cancellationToken));
    }

    public async Task<List<CmsUser>> GetAuthorsHasBlogPosts(CancellationToken cancellationToken = default)
    {
        return await (await GetDbContextAsync()).BlogPosts
            .Where(x => x.Status == BlogPostStatus.Published)
            .Select(x => x.Author).Distinct()
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
