using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BaseManagement
{
    public interface IPublicProductAppService : IApplicationService
    {
        Task<ListResultDto<BaseTypeDto>> GetListAsync();
    }
}