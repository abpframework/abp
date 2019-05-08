using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ProductManagement
{
    public interface IPublicProductAppService : IApplicationService
    {
        Task<ListResultDto<ProductDto>> GetListAsync();
    }
}