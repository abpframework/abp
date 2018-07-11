using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace MyCompanyName.MyModuleName
{
    public abstract class MyEntityRepository_Tests<TStartupModule> : MyModuleNameTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        [Fact]
        public async Task Test1()
        {

        }
    }
}
