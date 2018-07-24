using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobsTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private BackgroundJobsTestData _testData;

        public BackgroundJobsTestDataBuilder(
            IGuidGenerator guidGenerator,
            BackgroundJobsTestData testData)
        {
            _guidGenerator = guidGenerator;
            _testData = testData;
        }

        public void Build()
        {
            
        }
    }
}