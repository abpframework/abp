using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace ProductManagement
{
    [RemoteService]
    [Area("productManagement")]
    [Route("api/productManagement/public/products")]
    public class PublicProductsController : AbpController, IPublicProductAppService
    {
        private readonly IPublicProductAppService _publicProductAppService;

        public PublicProductsController(IPublicProductAppService publicProductAppService)
        {
            _publicProductAppService = publicProductAppService;
        }

        [HttpGet]
        public Task<ListResultDto<ProductDto>> GetListAsync()
        {
            return _publicProductAppService.GetListAsync();
        }
    }
}
