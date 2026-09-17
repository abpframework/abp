using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB;

public class MongoPermissionGrantRepository :
    MongoDbRepository<IPermissionManagementMongoDbContext, PermissionGrant, Guid>,
    IPermissionGrantRepository
{
    public MongoPermissionGrantRepository(IMongoDbContextProvider<IPermissionManagementMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<PermissionGrant> FindAsync(
        string name,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(s =>
                s.Name == name &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey,
                cancellationToken
            );
    }

    public virtual async Task<List<PermissionGrant>> GetListAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync(cancellationToken))
            .Where(s =>
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<PermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync(cancellationToken))
            .Where(s =>
                names.AsEnumerable().Contains(s.Name) &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(cancellationToken);
    }
}
