using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;

public class FakePermissionChecker : IPermissionChecker
{
    private HashSet<string>? _grantedPermissions;

    public void GrantAllPermissions()
    {
        _grantedPermissions = null;
    }

    public void SetGrantedPermissions(params string[] permissions)
    {
        _grantedPermissions = new HashSet<string>(permissions);
    }

    private bool IsGranted(string name)
    {
        return _grantedPermissions == null || _grantedPermissions.Contains(name);
    }

    public Task<bool> IsGrantedAsync(string name)
    {
        return Task.FromResult(IsGranted(name));
    }

    public Task<bool> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string name)
    {
        return Task.FromResult(IsGranted(name));
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names)
    {
        return IsGrantedAsync(null, names);
    }

    public Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string[] names)
    {
        var result = new MultiplePermissionGrantResult();
        foreach (var name in names)
        {
            result.Result[name] = IsGranted(name)
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }
        return Task.FromResult(result);
    }
}
