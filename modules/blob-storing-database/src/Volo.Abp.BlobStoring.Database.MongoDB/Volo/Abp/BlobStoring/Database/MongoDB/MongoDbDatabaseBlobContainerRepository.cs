using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    public class MongoDbDatabaseBlobContainerRepository : MongoDbRepository<IBlobStoringMongoDbContext, DatabaseBlobContainer, Guid>, IDatabaseBlobContainerRepository
    {
        public MongoDbDatabaseBlobContainerRepository(IMongoDbContextProvider<IBlobStoringMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
        
        public virtual async Task<DatabaseBlobContainer> FindAsync(string name, CancellationToken cancellationToken = default)
        {
            return await base.FindAsync(x => x.Name == name, cancellationToken: GetCancellationToken(cancellationToken));
        }
    }
}