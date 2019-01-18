using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace ProductManagement
{
    public abstract class MyEntityRepository_Tests<TStartupModule> : ProductManagementTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        [Fact]
        public async Task Test1()
        {

        }
    }
}
