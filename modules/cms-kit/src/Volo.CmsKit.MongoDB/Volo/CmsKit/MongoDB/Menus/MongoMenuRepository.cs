using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.MongoDB.Menus
{
    public class MongoMenuRepository : MongoDbRepository<ICmsKitMongoDbContext, Menu, Guid>, IMenuRepository
    {
        public MongoMenuRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
