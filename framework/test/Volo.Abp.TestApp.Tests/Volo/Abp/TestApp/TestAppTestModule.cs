using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;

namespace Volo.Abp.TestApp
{
    [DependsOn(typeof(MemoryDbTestModule))]
    public class TestAppTestModule : AbpModule
    {

    }
}
