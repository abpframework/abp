using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace ProductManagement
{
    public class ProductManagementTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private ProductManagementTestData _testData;

        public ProductManagementTestDataBuilder(
            IGuidGenerator guidGenerator,
            ProductManagementTestData testData)
        {
            _guidGenerator = guidGenerator;
            _testData = testData;
        }

        public void Build()
        {
            
        }
    }
}