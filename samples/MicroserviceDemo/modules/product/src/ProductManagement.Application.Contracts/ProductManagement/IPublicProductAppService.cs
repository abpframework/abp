using System.Threading.Tasks;
using ProductManagement;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyCompanyName.ProductManagement
{
    public interface IPublicProductAppService : IApplicationService
    {
        Task<ListResultDto<ProductDto>> GetListAsync();
    }
}