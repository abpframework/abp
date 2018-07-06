using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;

namespace Volo.Abp.TestApp
{
    [DependsOn(typeof(AbpMemoryDbTestModule))]
    public class TestAppTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<TestAppTestModule>();
        }
    }
}
