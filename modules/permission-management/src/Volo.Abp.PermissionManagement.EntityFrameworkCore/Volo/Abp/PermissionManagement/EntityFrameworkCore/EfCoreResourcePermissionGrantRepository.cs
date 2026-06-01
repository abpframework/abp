using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.PermissionManagement.EntityFrameworkCore;

public class EfCoreResourcePermissionGrantRepository : EfCoreRepository<IPermissionManagementDbContext, ResourcePermissionGrant, Guid>, IResourcePermissionGrantRepository
{
    public EfCoreResourcePermissionGrantRepository(IDbContextProvider<IPermissionManagementDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<ResourcePermissionGrant> FindAsync(string name, string resourceName, string resourceKey, string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync(s =>
                    s.Name == name &&
                    s.ResourceName == resourceName &&
                    s.ResourceKey == resourceKey &&
                    s.ProviderName == providerName &&
                    s.ProviderKey == providerKey,
                GetCancellationToken(cancellationToken)
            );
    }

    public virtual async Task<List<ResourcePermissionGrant>> GetListAsync(string resourceName, string resourceKey, string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                s.ResourceName == resourceName &&
                s.ResourceKey == resourceKey &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<ResourcePermissionGrant>> GetListAsync(string[] names, string resourceName, string resourceKey, string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                names.Contains(s.Name) &&
                s.ResourceName == resourceName &&
                s.ResourceKey == resourceKey &&
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<ResourcePermissionGrant>> GetListAsync(string providerName, string providerKey, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                s.ProviderName == providerName &&
                s.ProviderKey == providerKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<ResourcePermissionGrant>> GetPermissionsAsync(string resourceName, string resourceKey, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                s.ResourceName == resourceName &&
                s.ResourceKey == resourceKey
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<ResourcePermissionGrant>> GetResourceKeys(string resourceName, string name, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(s =>
                s.ResourceName == resourceName &&
                s.Name == name
            ).ToListAsync(GetCancellationToken(cancellationToken));
    }
}
