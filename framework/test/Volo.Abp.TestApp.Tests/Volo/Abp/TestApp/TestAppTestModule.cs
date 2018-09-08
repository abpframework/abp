using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;

namespace Volo.Abp.TestApp
{
    [DependsOn(typeof(AbpMemoryDbTestModule))]
    public class TestAppTestModule : AbpModule
    {

    }
}
