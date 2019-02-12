using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ProductManagement
{
    [Authorize(ProductManagementPermissions.Products.Default)]
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly ProductManager _productManager;
        private readonly IProductRepository _productRepository;

        public ProductAppService(ProductManager productManager, IProductRepository productRepository)
        {
            _productManager = productManager;
            _productRepository = productRepository;
        }

        public async Task<PagedResultDto<ProductDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input)
        {
            await NormalizeMaxResultCountAsync(input);

            var products = await _productRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount);

            var totalCount = await _productRepository.GetCountAsync();

            var dtos = ObjectMapper.Map<List<Product>, List<ProductDto>>(products);

            return new PagedResultDto<ProductDto>(totalCount, dtos);
        }

        public async Task<ListResultDto<ProductDto>> GetListAsync() //TODO: Why there are two GetList. GetListPagedAsync would be enough (rename it to GetList)!
        {
            var products = await _productRepository.GetListAsync();

            var productList =  ObjectMapper.Map<List<Product>, List<ProductDto>>(products);

            return new ListResultDto<ProductDto>(productList);
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            var product = await _productRepository.GetAsync(id);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        [Authorize(ProductManagementPermissions.Products.Create)]
        public async Task<ProductDto> CreateAsync(CreateProductDto input)
        {
            var product = await _productManager.CreateAsync(input.Code, input.Name, input.Price, input.StockCount);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        [Authorize(ProductManagementPermissions.Products.Update)]
        public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto input)
        {
            var product = await _productRepository.GetAsync(id);

            product.SetName(input.Name);
            product.SetPrice(input.Price);
            product.SetStockCount(input.StockCount);

            return ObjectMapper.Map<Product, ProductDto>(product);
        }

        [Authorize(ProductManagementPermissions.Products.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }

        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(ProductManagementSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}