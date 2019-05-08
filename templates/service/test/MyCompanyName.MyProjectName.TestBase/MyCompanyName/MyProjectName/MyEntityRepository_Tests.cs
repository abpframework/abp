using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace MyCompanyName.MyProjectName
{
    public abstract class MyEntityRepository_Tests<TStartupModule> : MyProjectNameTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        [Fact]
        public async Task Test1()
        {

        }
    }
}
