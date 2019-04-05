using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Docs
{
    public class DocsTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private DocsTestData _testData;

        public DocsTestDataBuilder(
            IGuidGenerator guidGenerator,
            DocsTestData testData)
        {
            _guidGenerator = guidGenerator;
            _testData = testData;
        }

        public void Build()
        {
            
        }
    }
}