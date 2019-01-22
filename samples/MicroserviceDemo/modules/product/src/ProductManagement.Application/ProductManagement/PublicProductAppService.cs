using System.Collections.Generic;
using System.Threading.Tasks;
using ProductManagement;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MyCompanyName.ProductManagement
{
    public class PublicProductAppService : ApplicationService, IPublicProductAppService
    {
        private readonly IProductRepository _productRepository;

        public PublicProductAppService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ListResultDto<ProductDto>> GetListAsync()
        {
            return new ListResultDto<ProductDto>(
                ObjectMapper.Map<List<Product>, List<ProductDto>>(
                    await _productRepository.GetListAsync()
                )
            );
        }
    }
}
