using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ProductManagement
{
    public class PublicProductAppService : ApplicationService, IPublicProductAppService
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public PublicProductAppService(IRepository<Product, Guid> productRepository)
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
