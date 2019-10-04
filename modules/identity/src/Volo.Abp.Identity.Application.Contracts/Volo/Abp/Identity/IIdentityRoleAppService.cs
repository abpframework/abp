using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public interface IIdentityRoleAppService : ICrudAppService<IdentityRoleDto, Guid, GetIdentityRolesInput, IdentityRoleCreateDto, IdentityRoleUpdateDto>
    {
        //TODO: remove after a better design
        Task<List<IdentityRoleDto>> GetAllListAsync();
    }
}
