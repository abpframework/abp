using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
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

        public async Task<Menu> FindMainMenuAsync(bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync()).FirstOrDefaultAsync(x => x.IsMainMenu);
        }
    }
}
