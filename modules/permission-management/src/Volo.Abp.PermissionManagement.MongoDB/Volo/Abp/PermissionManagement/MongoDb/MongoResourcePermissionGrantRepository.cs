using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB;

public class MongoResourcePermissionGrantRepository : MongoDbRepository<IPermissionManagementMongoDbContext, ResourcePermissionGrant, Guid>, IResourcePermissionGrantRepository
{
    public MongoResourcePermissionGrantRepository(IMongoDbContextProvider<IPermissionManagementMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<ResourcePermissionGrant> FindAsync(string name, string resourceName, string resourceKey, string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync(cancellationToken))
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(s =>
                    s.Name == name &&
                    s.ResourceName == resourceName &&
                    s.ResourceKey == resourceKey &&
                    s.ProviderName == providerName &&
                    s.ProviderKey == providerKey,
                cancellationToken
            );
    }

    public virtual async Task<List<ResourcePermissionGrant>> GetListAsync(string resourceName, string resourceKey, string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync(cancellationToken))
            .Where(s =>
                s.ResourceName == resourceName &&
                s.ResourceKey == resourceKey &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<ResourcePermissionGrant>> GetListAsync(string[] names, string resourceName, string resourceKey, string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync(cancellationToken))
            .Where(s =>
                names.AsEnumerable().Contains(s.Name) &&
                s.ResourceName == resourceName &&
                s.ResourceKey == resourceKey &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<ResourcePermissionGrant>> GetListAsync(string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync(cancellationToken))
            .Where(s =>
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<ResourcePermissionGrant>> GetPermissionsAsync(string resourceName, string resourceKey, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync(cancellationToken))
            .Where(s =>
                s.ResourceName == resourceName &&
                s.ResourceKey == resourceKey
            ).ToListAsync(cancellationToken);
    }

    public virtual async Task<List<ResourcePermissionGrant>> GetResourceKeys(string resourceName, string name, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetQueryableAsync(cancellationToken))
            .Where(s =>
                s.ResourceName == resourceName &&
                s.Name == name
            ).ToListAsync(cancellationToken);
    }
}
