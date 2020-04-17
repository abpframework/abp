using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ProductManagement
{
    public class ProductManager_Tests : ProductManagementDomainTestBase
    {
        private readonly ProductManager _productManager;

        public ProductManager_Tests()
        {
            _productManager = GetRequiredService<ProductManager>();
        }

        [Fact]
        public async Task Should_Create_A_Valid_Product()
        {
            //Act
            var product = await WithUnitOfWorkAsync(
                async () =>
                {
                    return await _productManager.CreateAsync("P000837212", "My Product 837212", stockCount: 42);
                }
            );

            //Assert
            product.Code.ShouldBe("P000837212");
            product.Name.ShouldBe("My Product 837212");
            product.Price.ShouldBe(0.0f);
            product.StockCount.ShouldBe(42);
        }
    }
}
