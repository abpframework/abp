using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.MongoDB.Tags
{
    public class MongoEntityTagRepository: MongoDbRepository<ICmsKitMongoDbContext, EntityTag>, IEntityTagRepository
    {
        public MongoEntityTagRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}