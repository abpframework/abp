using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Logging;
using Volo.Abp.Modularity.PlugIns;
using Xunit;

namespace Volo.Abp.Modularity;

public class ModuleLoader_Tests
{
    [Fact]
    public void Should_Load_Modules_By_Dependency_Order()
    {
        var moduleLoader = new ModuleLoader();
        var modules = moduleLoader.LoadModules(
            new ServiceCollection()
                .AddSingleton<IInitLoggerFactory>(new DefaultInitLoggerFactory()),
            typeof(MyStartupModule),
            new PlugInSourceList()
        );
        modules.Length.ShouldBe(2);
        modules[0].Type.ShouldBe(typeof(IndependentEmptyModule));
        modules[1].Type.ShouldBe(typeof(MyStartupModule));
    }

    [DependsOn(typeof(IndependentEmptyModule))]
    public class MyStartupModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
