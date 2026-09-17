using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.TestServices.Resources;

public class FakeResourcePermissionStore : IResourcePermissionStore, ITransientDependency
{
    public Task<bool> IsGrantedAsync(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        return Task.FromResult((name == "MyResourcePermission3" || name == "MyResourcePermission5") &&
                               resourceName == TestEntityResource.ResourceName &&
                               (resourceKey == TestEntityResource.ResourceKey3 || resourceKey == TestEntityResource.ResourceKey5));
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var result = new MultiplePermissionGrantResult();
        foreach (var name in names)
        {
            result.Result.Add(name, ((name == "MyResourcePermission3" || name == "MyResourcePermission5") &&
                resourceName == TestEntityResource.ResourceName &&
                (resourceKey == TestEntityResource.ResourceKey3 || resourceKey == TestEntityResource.ResourceKey5)
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Prohibited));
        }

        return Task.FromResult(result);
    }

    public Task<MultiplePermissionGrantResult> GetPermissionsAsync(string resourceName, string resourceKey)
    {
        throw new System.NotImplementedException();
    }

    public Task<string[]> GetGrantedPermissionsAsync(string resourceName, string resourceKey)
    {
        throw new System.NotImplementedException();
    }

    public Task<string[]> GetGrantedResourceKeysAsync(string resourceName, string name)
    {
        throw new System.NotImplementedException();
    }
}
