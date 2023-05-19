using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.PermissionManagement;

public class TestRequireRolePermissionStateProvider : ISimpleStateChecker<PermissionDefinition>
{
    private readonly List<string> _allowRoles = new List<string>();

    public TestRequireRolePermissionStateProvider(params string[] roles)
    {
        _allowRoles.AddRange(roles);
    }

    public Task<bool> IsEnabledAsync(SimpleStateCheckerContext<PermissionDefinition> context)
    {
        var currentPrincipalAccessor = context.ServiceProvider.GetRequiredService<ICurrentPrincipalAccessor>();
        return Task.FromResult(currentPrincipalAccessor.Principal != null && _allowRoles.Any(role => currentPrincipalAccessor.Principal.IsInRole(role)));
    }
}
