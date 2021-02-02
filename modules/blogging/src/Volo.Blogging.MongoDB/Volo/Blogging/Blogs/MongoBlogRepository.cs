using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Blogging.MongoDB;

namespace Volo.Blogging.Blogs
{
    public class MongoBlogRepository : MongoDbRepository<IBloggingMongoDbContext, Blog, Guid>, IBlogRepository
    {
        public MongoBlogRepository(IMongoDbContextProvider<IBloggingMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Blog> FindByShortNameAsync(string shortName, CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken)).FirstOrDefaultAsync(p => p.ShortName == shortName, GetCancellationToken(cancellationToken));
        }
    }
}
