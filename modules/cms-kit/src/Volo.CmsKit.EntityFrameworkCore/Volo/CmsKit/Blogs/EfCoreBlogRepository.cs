using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Blogs;

public class EfCoreBlogRepository : EfCoreRepository<ICmsKitDbContext, Blog, Guid>, IBlogRepository
{
    public EfCoreBlogRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).AnyAsync(x => x.Id == id, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.Slug == slug, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Blog>> GetListAsync(
        string filter = null,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = await GetListQueryAsync(filter);

        return await query.OrderBy(sorting.IsNullOrEmpty() ? "creationTime desc" : sorting)
                          .PageBy(skipCount, maxResultCount)
                          .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<BlogWithBlogPostCount>> GetListWithBlogPostCountAsync(
        string filter = null,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var blogs = await (await GetListQueryAsync(filter)).OrderBy(sorting.IsNullOrEmpty() ? "creationTime desc" : sorting)
            .PageBy(skipCount, maxResultCount).ToListAsync(GetCancellationToken(cancellationToken));

        var blogIds = blogs.Select(x => x.Id).ToArray();
        
        var blogPostCount = await (await GetDbContextAsync()).Set<BlogPost>()
            .Where(blogPost => blogIds.Contains(blogPost.BlogId))
            .GroupBy(blogPost => blogPost.BlogId)
            .Select(x => new
            {
                BlogId = x.Key,
                Count = x.Count()
            })
            .ToListAsync(GetCancellationToken(cancellationToken));

        return blogs.Select(blog => new BlogWithBlogPostCount(blog, blogPostCount.FirstOrDefault(x => x.BlogId == blog.Id)?.Count ?? 0)).ToList();
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        var query = await GetListQueryAsync(filter);

        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual Task<Blog> GetBySlugAsync([NotNull] string slug, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(slug, nameof(slug));
        return GetAsync(x => x.Slug == slug, cancellationToken: GetCancellationToken(cancellationToken));
    }

    protected virtual async Task<IQueryable<Blog>> GetListQueryAsync(string filter = null)
    {
        return (await GetDbSetAsync())
            .WhereIf(!filter.IsNullOrWhiteSpace(), b => b.Name.Contains(filter));
    }
}
