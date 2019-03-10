using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Abp.FeatureManagement
{
    public class FeatureManagementTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private FeatureManagementTestData _testData;

        public FeatureManagementTestDataBuilder(
            IGuidGenerator guidGenerator,
            FeatureManagementTestData testData)
        {
            _guidGenerator = guidGenerator;
            _testData = testData;
        }

        public void Build()
        {
            
        }
    }
}