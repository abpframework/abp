using System;
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
    }
}