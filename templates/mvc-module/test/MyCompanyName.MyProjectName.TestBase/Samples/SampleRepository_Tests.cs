using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace MyCompanyName.MyProjectName.Samples
{
    public abstract class SampleRepository_Tests<TStartupModule> : MyProjectNameTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        //private readonly ISampleRepository _sampleRepository;

        protected SampleRepository_Tests()
        {
            //_sampleRepository = GetRequiredService<ISampleRepository>();
        }

        [Fact]
        public async Task Method1Async()
        {

        }
    }
}
