using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace ProductManagement
{
    public class ProductAppService_Tests : ProductManagementApplicationTestBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IProductRepository _productRepository;
        private readonly ProductManagementTestData _testData;

        public ProductAppService_Tests()
        {
            _productAppService = GetRequiredService<IProductAppService>();
            _productRepository = GetRequiredService<IProductRepository>();
            _testData = GetRequiredService<ProductManagementTestData>();
        }

        [Fact]
        public async Task GetListPagedAsync()
        {
            var result = await _productAppService.GetListPagedAsync(new PagedAndSortedResultRequestDto());

            result.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetListAsync()
        {
            var result = await _productAppService.GetListAsync();

            result.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetAsync()
        {
            var result = await _productAppService.GetAsync(_testData.ProductId1);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(_testData.ProductName1);
            result.Code.ShouldBe(_testData.ProductCode1);
            result.Price.ShouldBe(_testData.ProductPrice1);
            result.StockCount.ShouldBe(_testData.ProductStockCount1);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var result = await _productAppService.CreateAsync(new CreateProductDto()
            {
                Code = "Code",
                Name = "Name",
                Price = 15,
                StockCount = 14
            });

            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var result = await _productAppService.UpdateAsync(_testData.ProductId1, new UpdateProductDto()
            {
                Code = nameof(Product.Code),
                Name = nameof(Product.Name),
                Price = 15,
                StockCount = 14
            });

            result.ShouldNotBeNull();
            result.Code.ShouldBe(nameof(Product.Code));
            result.Name.ShouldBe(nameof(Product.Name));
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await _productAppService.DeleteAsync(_testData.ProductId2);

            var result = await _productRepository.GetAsync(_testData.ProductId2);

            result.ShouldBeNull();
        }

    }
}
