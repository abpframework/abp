using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.BackgroundJobs
{
    public abstract class MyEntityRepository_Tests<TStartupModule> : BackgroundJobsTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        [Fact]
        public async Task Test1()
        {

        }
    }
}
