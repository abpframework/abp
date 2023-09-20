using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity.Integration;

namespace Volo.Abp.Identity;

[Dependency(TryRegister = true)]
public class HttpClientUserRoleFinder : IUserRoleFinder, ITransientDependency
{
    protected IIdentityUserAppService _userAppService { get; }
    protected IIdentityUserIntegrationService _userIntegrationService { get; }

    public HttpClientUserRoleFinder(IIdentityUserAppService userAppService, IIdentityUserIntegrationService userIntegrationService)
    {
        _userAppService = userAppService;
        _userIntegrationService = userIntegrationService;
    }

    [Obsolete("Use GetRoleNamesAsync instead.")]
    public virtual async Task<string[]> GetRolesAsync(Guid userId)
    {
        var output = await _userAppService.GetRolesAsync(userId);
        return output.Items.Select(r => r.Name).ToArray();
    }

    public async Task<string[]> GetRoleNamesAsync(Guid userId)
    {
        return await _userIntegrationService.GetRoleNamesAsync(userId);
    }
}
