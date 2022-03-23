using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Blogs;

public class EfCoreBlogFeatureRepository : EfCoreRepository<CmsKitDbContext, BlogFeature, Guid>, IBlogFeatureRepository
{
    public EfCoreBlogFeatureRepository(IDbContextProvider<CmsKitDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public Task<BlogFeature> FindAsync(Guid blogId, string featureName, CancellationToken cancellationToken = default)
    {
        return base.FindAsync(x => x.BlogId == blogId && x.FeatureName == featureName, cancellationToken: cancellationToken);
    }

    public async Task<List<BlogFeature>> GetListAsync(Guid blogId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
                        .Where(x => x.BlogId == blogId)
                        .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task<List<BlogFeature>> GetListAsync(Guid blogId, List<string> featureNames, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
                    .Where(x => x.BlogId == blogId && featureNames.Contains(x.FeatureName))
                    .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
