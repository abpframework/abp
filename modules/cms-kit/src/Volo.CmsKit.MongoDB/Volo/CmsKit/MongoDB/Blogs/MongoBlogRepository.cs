using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.MongoDB.Blogs;

public class MongoBlogRepository : MongoDbRepository<ICmsKitMongoDbContext, Blog, Guid>, IBlogRepository
{
    public MongoBlogRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);
        return await (await GetMongoQueryableAsync(token)).AnyAsync(x => x.Id == id, token);
    }

    public virtual async Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);
        return await (await GetMongoQueryableAsync(token)).AnyAsync(x => x.Slug == slug, token);
    }

    public virtual async Task<List<Blog>> GetListAsync(
        string filter = null,
        string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        var query = await GetListQueryAsync(filter, token);

        return await query.OrderBy(sorting.IsNullOrEmpty() ? "creationTime desc" : sorting)
                  .As<IMongoQueryable<Blog>>()
                  .PageBy<Blog, IMongoQueryable<Blog>>(skipCount, maxResultCount)
                  .ToListAsync(token);
    }

    public virtual async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        var token = GetCancellationToken(cancellationToken);

        var query = await GetListQueryAsync(filter, token);

        return await query.As<IMongoQueryable<Blog>>().LongCountAsync(token);
    }

    public virtual Task<Blog> GetBySlugAsync([NotNull] string slug, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrEmpty(slug, nameof(slug));
        return GetAsync(x => x.Slug == slug, cancellationToken: cancellationToken);
    }

    protected virtual async Task<IQueryable<Blog>> GetListQueryAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        return (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf(!filter.IsNullOrWhiteSpace(), b => b.Name.Contains(filter));
    }
}
