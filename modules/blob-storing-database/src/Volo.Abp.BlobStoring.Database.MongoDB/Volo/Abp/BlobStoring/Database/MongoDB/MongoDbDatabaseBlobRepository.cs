using MongoDB.Driver.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    public class MongoDbDatabaseBlobRepository : MongoDbRepository<IBlobStoringMongoDbContext, DatabaseBlob, Guid>, IDatabaseBlobRepository
    {
        public MongoDbDatabaseBlobRepository(IMongoDbContextProvider<IBlobStoringMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<DatabaseBlob> FindAsync(Guid containerId, string name, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().FirstOrDefaultAsync(
                    x => x.ContainerId == containerId &&
                         x.Name == name,
                    GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> ExistsAsync(Guid containerId, string name, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().AnyAsync(
                x => x.ContainerId == containerId &&
                     x.Name == name,
                GetCancellationToken(cancellationToken));
        }

        public virtual async Task<bool> DeleteAsync(
            Guid containerId, 
            string name,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            var blob = await FindAsync(containerId, name, cancellationToken);

            if (blob == null)
            {
                return false;
            }

            await base.DeleteAsync(blob, autoSave, cancellationToken: GetCancellationToken(cancellationToken));
            return true;
        }
    }
}