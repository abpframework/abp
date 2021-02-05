using MongoDB.Driver.Core.Operations;
using System;
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

        public async Task<bool> ExistsAsync(Guid blogId)
        {
            return await AsyncExecuter.AnyAsync(
                            await GetQueryableAsync(),
                            x => x.Id == blogId);
        }

        public Task<Blog> GetByUrlSlugAsync(string urlSlug)
        {
            return GetAsync(x => x.UrlSlug == urlSlug);
        }
    }
}
