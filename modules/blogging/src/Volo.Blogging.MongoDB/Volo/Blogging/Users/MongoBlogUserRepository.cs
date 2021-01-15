using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MongoDB;
using Volo.Abp.Users.MongoDB;
using Volo.Blogging.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Volo.Blogging.Users
{
    public class MongoBlogUserRepository : MongoUserRepositoryBase<IBloggingMongoDbContext, BlogUser>, IBlogUserRepository
    {
        public MongoBlogUserRepository(IMongoDbContextProvider<IBloggingMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<BlogUser>> GetUsersAsync(int maxCount, string filter, CancellationToken cancellationToken)
        {
            var query = await GetMongoQueryableAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(x => x.UserName.Contains(filter));
            }

            return await query.Take(maxCount).ToListAsync(cancellationToken);
        }
    }
}
