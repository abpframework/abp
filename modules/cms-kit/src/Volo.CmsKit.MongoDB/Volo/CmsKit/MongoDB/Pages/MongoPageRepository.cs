using System;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.MongoDB.Pages
{
    public class MongoPageRepository : MongoDbRepository<ICmsKitMongoDbContext, Page, Guid>, IPageRepository
    {
        public MongoPageRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<Page> GetByUrlAsync(string url)
        {
            return GetAsync(x => x.Url == url);
        }

        public Task<Page> FindByUrlAsync(string url)
        {
            return FindAsync(x => x.Url == url);
        }

        public async Task<bool> DoesExistAsync(string url)
        {
            return await (await GetMongoQueryableAsync()).AnyAsync(x => x.Url == url);
        }
    }
}
