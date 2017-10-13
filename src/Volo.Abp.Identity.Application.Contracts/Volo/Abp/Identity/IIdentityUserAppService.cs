using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public interface IIdentityUserAppService : IAsyncCrudAppService<IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>
    {
        Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);

        Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);
    }
}
