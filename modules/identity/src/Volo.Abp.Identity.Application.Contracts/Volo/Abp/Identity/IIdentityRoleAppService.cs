using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public interface IIdentityRoleAppService
        : ICrudAppService<
            IdentityRoleDto,
            Guid,
            PagedAndSortedResultRequestDto,
            IdentityRoleCreateDto,
            IdentityRoleUpdateDto>
    {
        Task<ListResultDto<IdentityRoleDto>> GetAllListAsync();
    }
}
