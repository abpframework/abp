using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public interface IIdentityRoleAppService : IApplicationService
    {
        Task<PagedResultDto<IdentityRoleDto>> GetListAsync(PagedAndSortedResultRequestDto input);

        Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input);

        Task<IdentityRoleDto> GetAsync(Guid id);

        Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input);

        Task DeleteAsync(Guid id);
    }
}
