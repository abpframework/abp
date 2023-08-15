using System;
using System.Threading.Tasks;

namespace Volo.Abp.Identity.Integration;

public class IdentityUserIntegrationService : IdentityAppServiceBase, IIdentityUserIntegrationService
{
    protected IUserRoleFinder UserRoleFinder { get; }

    public IdentityUserIntegrationService(IUserRoleFinder userRoleFinder)
    {
        UserRoleFinder = userRoleFinder;
    }

    public virtual async Task<string[]> GetRoleNamesAsync(Guid id)
    {
        return await UserRoleFinder.GetRoleNamesAsync(id);
    }
}
