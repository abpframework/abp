using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Authorization.Permissions.Resources;

public class NullResourcePermissionStore : IResourcePermissionStore, ISingletonDependency
{
    public ILogger<NullResourcePermissionStore> Logger { get; set; }

    public NullResourcePermissionStore()
    {
        Logger = NullLogger<NullResourcePermissionStore>.Instance;
    }

    public Task<bool> IsGrantedAsync(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        return TaskCache.FalseResult;
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        return Task.FromResult(new MultiplePermissionGrantResult(names, PermissionGrantResult.Prohibited));
    }

    public Task<MultiplePermissionGrantResult> GetPermissionsAsync(string resourceName, string resourceKey)
    {
        return Task.FromResult(new MultiplePermissionGrantResult());
    }

    public Task<string[]> GetGrantedPermissionsAsync(string resourceName, string resourceKey)
    {
        return Task.FromResult(Array.Empty<string>());
    }

    public Task<string[]> GetGrantedResourceKeysAsync(string resourceName, string name)
    {
        return Task.FromResult(Array.Empty<string>());
    }
}
