using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.PermissionManagement;

namespace Volo.Abp.Identity
{
    public interface IIdentityUserAppService : IAsyncCrudAppService<IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>
    {
        Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);

        Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);

        Task<GetPermissionListResultDto> GetPermissionsAsync(Guid id);

        Task UpdatePermissionsAsync(Guid id, UpdatePermissionsDto input);

        Task<IdentityUserDto> FindByUsernameAsync(string username);

        Task<IdentityUserDto> FindByEmailAsync(string email);
    }
}
