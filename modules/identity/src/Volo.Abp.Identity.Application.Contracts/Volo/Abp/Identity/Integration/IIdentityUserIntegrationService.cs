using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace Volo.Abp.Identity.Integration;

[IntegrationService]
public interface IIdentityUserIntegrationService : IApplicationService
{
    Task<string[]> GetRoleNamesAsync(Guid id);

    Task<UserData> FindByIdAsync(Guid id);

    Task<UserData> FindByUserNameAsync(string userName);

    Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input);

    Task<ListResultDto<UserData>> SearchByIdsAsync(Guid[] ids);

    Task<long> GetCountAsync(UserLookupCountInputDto input);

    Task<ListResultDto<RoleData>> SearchRoleAsync(RoleLookupSearchInputDto input);

    Task<ListResultDto<RoleData>> SearchRoleByNamesAsync(string[] ids);

    Task<long> GetRoleCountAsync(RoleLookupCountInputDto input);
}
