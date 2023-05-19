using MongoDB.Driver.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB;

public class MongoDbDatabaseBlobRepository : MongoDbRepository<IBlobStoringMongoDbContext, DatabaseBlob, Guid>, IDatabaseBlobRepository
{
    public MongoDbDatabaseBlobRepository(IMongoDbContextProvider<IBlobStoringMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<DatabaseBlob> FindAsync(Guid containerId, string name, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(cancellationToken))
            .FirstOrDefaultAsync(
                x => x.ContainerId == containerId && x.Name == name,
                cancellationToken
            );
    }

    public virtual async Task<bool> ExistsAsync(Guid containerId, string name, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(cancellationToken))
            .AnyAsync(
                x => x.ContainerId == containerId && x.Name == name,
                cancellationToken
            );
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

        await base.DeleteAsync(blob, autoSave, cancellationToken);

        return true;
    }
}
