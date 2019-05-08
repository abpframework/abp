using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace ProductManagement
{
    [RemoteService]
    [Area("productManagement")]
    [Route("api/productManagement/products")]
    public class ProductsController : AbpController, IProductAppService
    {
        private readonly IProductAppService _productAppService;

        public ProductsController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpGet]
        [Route("")]
        public Task<PagedResultDto<ProductDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input)
        {
            return _productAppService.GetListPagedAsync(input);
        }

        [HttpGet]
        [Route("all")]
        public Task<ListResultDto<ProductDto>> GetListAsync()
        {
            return _productAppService.GetListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public Task<ProductDto> GetAsync(Guid id)
        {
            return _productAppService.GetAsync(id);
        }

        [HttpPost]
        public Task<ProductDto> CreateAsync(CreateProductDto input)
        {
            return _productAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto input)
        {
            return _productAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public Task DeleteAsync(Guid id)
        {
            return _productAppService.DeleteAsync(id);
        }
    }
}
