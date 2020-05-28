using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    public class MongoContainerRepository : MongoDbRepository<IBlobStoringDatabaseMongoDbContext, Container, Guid>, IContainerRepository
    {
        public MongoContainerRepository(IMongoDbContextProvider<IBlobStoringDatabaseMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<Container> GetContainerAsync(string name, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().Where(x => x.Name == name).FirstAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Container> FindContainerAsync(string name, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().Where(x => x.Name == name).FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }
    }
}