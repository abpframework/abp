using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace MyCompanyName.MyProjectName
{
    public class MyProjectNameTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private MyProjectNameTestData _testData;

        public MyProjectNameTestDataBuilder(
            IGuidGenerator guidGenerator,
            MyProjectNameTestData testData)
        {
            _guidGenerator = guidGenerator;
            _testData = testData;
        }

        public void Build()
        {
            
        }
    }
}