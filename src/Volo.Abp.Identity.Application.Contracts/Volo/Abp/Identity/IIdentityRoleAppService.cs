using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    public interface IIdentityRoleAppService : IAsyncCrudAppService<IdentityRoleDto, Guid, GetIdentityRolesInput, IdentityRoleCreateDto, IdentityRoleUpdateDto>
    {
        //TODO: remove after a better design
        Task<List<IdentityRoleDto>> GetAllListAsync();

        Task<GetPermissionListResultDto> GetPermissionsAsync(Guid id);

        Task UpdatePermissionsAsync(Guid id, UpdatePermissionsDto input);
    }
}
