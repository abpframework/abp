using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.MongoDB.Tags
{
    public class MongoTagRepository : MongoDbRepository<ICmsKitMongoDbContext, Tag, Guid>, ITagRepository
    {
        public MongoTagRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
