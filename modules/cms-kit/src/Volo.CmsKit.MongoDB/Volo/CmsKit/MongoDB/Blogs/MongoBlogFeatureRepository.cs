using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;
using MongoDB.Driver;

namespace Volo.CmsKit.MongoDB.Blogs;

public class MongoBlogFeatureRepository : MongoDbRepository<ICmsKitMongoDbContext, BlogFeature, Guid>, IBlogFeatureRepository
{
    public MongoBlogFeatureRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public Task<BlogFeature> FindAsync(Guid blogId, string featureName, CancellationToken cancellationToken = default)
    {
        return base.FindAsync(x => x.BlogId == blogId && x.FeatureName == featureName, cancellationToken: cancellationToken);
    }

    public virtual async Task<List<BlogFeature>> GetListAsync(Guid blogId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
                        .Where(x => x.BlogId == blogId)
                        .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<BlogFeature>> GetListAsync(Guid blogId, List<string> featureNames, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
                    .Where(x => x.BlogId == blogId && featureNames.Contains(x.FeatureName))
                    .ToListAsync(GetCancellationToken(cancellationToken));
    }
}
