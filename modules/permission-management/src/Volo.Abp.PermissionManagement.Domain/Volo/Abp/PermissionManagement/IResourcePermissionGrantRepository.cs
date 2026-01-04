using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.PermissionManagement;

public interface IResourcePermissionGrantRepository : IBasicRepository<ResourcePermissionGrant, Guid>
{
    Task<ResourcePermissionGrant> FindAsync(
        string name,
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<ResourcePermissionGrant>> GetListAsync(
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<ResourcePermissionGrant>> GetListAsync(
        string[] names,
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<ResourcePermissionGrant>> GetListAsync(
        string providerName,
        string providerKey,
        CancellationToken cancellationToken = default
    );

    Task<List<ResourcePermissionGrant>> GetPermissionsAsync(
        string resourceName,
        string resourceKey,
        CancellationToken cancellationToken = default
    );

    Task<List<ResourcePermissionGrant>> GetResourceKeys(
        string resourceName,
        string name,
        CancellationToken cancellationToken = default
    );
}
