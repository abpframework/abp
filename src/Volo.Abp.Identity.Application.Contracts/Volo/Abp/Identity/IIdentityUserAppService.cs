using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public interface IIdentityUserAppService : IAsyncCrudAppService<IdentityUserDto, Guid, PagedAndSortedResultRequestDto, IdentityUserCreateOrUpdateDto>
    {
        
    }
}
