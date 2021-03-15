using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;
using MongoDB.Driver;

namespace Volo.CmsKit.MongoDB.Blogs
{
    public class MongoBlogFeatureRepository : MongoDbRepository<ICmsKitMongoDbContext, BlogFeature, Guid> , IBlogFeatureRepository
    {
        public MongoBlogFeatureRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<BlogFeature> FindAsync(Guid blogId, string featureName)
        {
            return base.FindAsync(x => x.BlogId == blogId && x.FeatureName == featureName);
        }

        public async Task<List<BlogFeature>> GetListAsync(Guid blogId)
        {
            return await (await GetMongoQueryableAsync())
                            .Where(x => x.BlogId == blogId)
                            .ToListAsync();
        }

        public async Task<List<BlogFeature>> GetListAsync(Guid blogId, List<string> featureNames)
        {
            return await (await GetMongoQueryableAsync())
                        .Where(x => x.BlogId == blogId && featureNames.Contains(x.FeatureName))
                        .ToListAsync();
        }
    }
}
