using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Services.Dtos;

namespace Volo.Abp.Identity
{
    public interface IUserAppService : IApplicationService
    {
        Task<ListResultDto<IdentityUserDto>> GetAll();

        Task<IdentityUserDto> Get(Guid id);
    }
}
