using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.MongoDB.Menus
{
    public class MongoMenuItemRepository : MongoDbRepository<ICmsKitMongoDbContext, MenuItem, Guid>, IMenuItemRepository
    {
        public MongoMenuItemRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
