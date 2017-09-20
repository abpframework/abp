using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public interface IIdentityRoleAppService : IAsyncCrudAppService<IdentityRoleDto, Guid, PagedAndSortedResultRequestDto, IdentityRoleCreateDto, IdentityRoleUpdateDto>
    {

    }
}
