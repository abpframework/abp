using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity
{
    public interface IUserAppService : IApplicationService
    {
        Task<ListResultDto<IdentityUserDto>> Get();

        Task<IdentityUserDto> Get(Guid id);
    }
}
