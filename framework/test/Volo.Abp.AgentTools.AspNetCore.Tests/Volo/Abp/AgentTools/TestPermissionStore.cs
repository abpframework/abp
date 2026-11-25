using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AgentTools;

/// <summary>
/// Permission grants for the test host (one host per test): grant permissions to a user id, then switch to that
/// user with <see cref="AgentToolsAspNetCoreTestBase.LoginAs"/>. Nothing is granted by default.
/// </summary>
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IPermissionStore), typeof(TestPermissionStore))]
public class TestPermissionStore : IPermissionStore, ISingletonDependency
{
    private readonly ConcurrentDictionary<(string Name, string ProviderName, string ProviderKey), bool> _grants = new();

    public void GrantToUser(Guid userId, params string[] permissionNames)
    {
        foreach (var name in permissionNames)
        {
            _grants[(name, UserPermissionValueProvider.ProviderName, userId.ToString())] = true;
        }
    }

    public void RevokeFromUser(Guid userId, params string[] permissionNames)
    {
        foreach (var name in permissionNames)
        {
            _grants.TryRemove((name, UserPermissionValueProvider.ProviderName, userId.ToString()), out _);
        }
    }

    public void Clear() => _grants.Clear();

    public Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
    {
        return Task.FromResult(_grants.ContainsKey((name, providerName, providerKey)));
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string providerName, string providerKey)
    {
        var result = new MultiplePermissionGrantResult();
        foreach (var name in names.Distinct())
        {
            result.Result[name] = _grants.ContainsKey((name, providerName, providerKey))
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }

        return Task.FromResult(result);
    }
}
