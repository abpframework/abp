using System;
using System.Threading.Tasks;

namespace Volo.Abp.Identity.Integration;

public class IdentityUserIntegrationService : IdentityAppServiceBase, IIdentityUserIntegrationService
{
    protected IIdentityUserRepository UserRepository { get; }

    public IdentityUserIntegrationService(IIdentityUserRepository userRepository)
    {
        UserRepository = userRepository;
    }

    public async Task<string[]> GetRoleNamesAsync(Guid id)
    {
        return (await UserRepository.GetRoleNamesAsync(id)).ToArray();
    }
}