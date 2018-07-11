using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace MyCompanyName.MyModuleName
{
    public class MyModuleNameTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private MyModuleNameTestData _testData;

        public MyModuleNameTestDataBuilder(
            IGuidGenerator guidGenerator,
            MyModuleNameTestData testData)
        {
            _guidGenerator = guidGenerator;
            _testData = testData;
        }

        public void Build()
        {
            
        }
    }
}