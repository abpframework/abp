using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.TestServices;

public class FakePermissionStore : IPermissionStore, ITransientDependency
{
    public Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
    {
        return Task.FromResult(name == "MyPermission3" || name == "MyPermission5");
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
    {
        var result = new MultiplePermissionGrantResult();
        foreach (var name in names)
        {
            result.Result.Add(name, name == "MyPermission3" || name == "MyPermission5"
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Prohibited);
        }

        return Task.FromResult(result);
    }
}
