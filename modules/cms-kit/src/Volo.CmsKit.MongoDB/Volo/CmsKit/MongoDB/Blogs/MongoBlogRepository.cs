using MongoDB.Driver.Core.Operations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.MongoDB.Blogs
{
    public class MongoBlogRepository : MongoDbRepository<ICmsKitMongoDbContext, Blog, Guid>, IBlogRepository
    {
        public MongoBlogRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> ExistsAsync(Guid blogId, CancellationToken cancellationToken = default)
        {
            return await AsyncExecuter.AnyAsync(
                            await GetQueryableAsync(),
                                x => x.Id == blogId, 
                                cancellationToken);
        }

        public virtual Task<Blog> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return GetAsync(x => x.Slug == slug, cancellationToken: cancellationToken);
        }
    }
}
