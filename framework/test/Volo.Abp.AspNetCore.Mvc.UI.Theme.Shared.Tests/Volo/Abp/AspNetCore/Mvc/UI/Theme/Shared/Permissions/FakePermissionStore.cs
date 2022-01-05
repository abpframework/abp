using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Tests.Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Permissions;

public class FakePermissionStore : IPermissionStore, ITransientDependency
{
    public Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
    {
        var result = (name.Contains("MyComponent1") || name.Contains("MyComponent3"));
        return Task.FromResult(result);
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
    {
        var result = new MultiplePermissionGrantResult();
        foreach (var name in names)
        {
            result.Result.Add(name, (name.Contains("MyComponent1") || name.Contains("MyComponent3"))
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Prohibited);
        }

        return Task.FromResult(result);
    }
}
