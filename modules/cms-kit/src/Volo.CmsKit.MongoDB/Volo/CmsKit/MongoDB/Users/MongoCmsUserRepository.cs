using Volo.Abp.MongoDB;
using Volo.Abp.Users.MongoDB;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.MongoDB.Users
{
    public class MongoCmsUserRepository : MongoUserRepositoryBase<ICmsKitMongoDbContext, CmsUser>, ICmsUserRepository
    {
        public MongoCmsUserRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
