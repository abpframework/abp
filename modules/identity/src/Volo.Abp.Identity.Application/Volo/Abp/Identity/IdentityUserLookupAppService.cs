using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity.Integration;
using Volo.Abp.Users;

namespace Volo.Abp.Identity;

[Obsolete("Use IdentityUserIntegrationService for module-to-module (or service-to-service) communication.")]
[Authorize(IdentityPermissions.UserLookup.Default)]
public class IdentityUserLookupAppService : IdentityAppServiceBase, IIdentityUserLookupAppService
{
    protected IIdentityUserIntegrationService IdentityUserIntegrationService { get; }

    public IdentityUserLookupAppService(
        IIdentityUserIntegrationService identityUserIntegrationService)
    {
        IdentityUserIntegrationService = identityUserIntegrationService;
    }

    public virtual async Task<UserData> FindByIdAsync(Guid id)
    {
        return await IdentityUserIntegrationService.FindByIdAsync(id);
    }

    public virtual async Task<UserData> FindByUserNameAsync(string userName)
    {
        return await IdentityUserIntegrationService.FindByUserNameAsync(userName);
    }

    public virtual async Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
    {
        return await IdentityUserIntegrationService.SearchAsync(input);
    }

    public virtual async Task<long> GetCountAsync(UserLookupCountInputDto input)
    {
        return await IdentityUserIntegrationService.GetCountAsync(input);
    }
}
