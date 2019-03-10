using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace Abp.FeatureManagement
{
    public abstract class MyEntityRepository_Tests<TStartupModule> : FeatureManagementTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        [Fact]
        public async Task Test1()
        {

        }
    }
}
