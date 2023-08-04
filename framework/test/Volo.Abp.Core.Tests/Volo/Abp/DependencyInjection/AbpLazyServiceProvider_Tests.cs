using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Testing.Utils;
using Xunit;

namespace Volo.Abp.DependencyInjection;

public class AbpLazyServiceProvider_Tests
{
    [Fact]
    public void LazyServiceProvider_Should_Cache_Services()
    {
        using (var application = AbpApplicationFactory.Create<TestModule>())
        {
            application.Initialize();

            var lazyServiceProvider = application.ServiceProvider.GetRequiredService<IAbpLazyServiceProvider>();

            var transientTestService1 = lazyServiceProvider.LazyGetRequiredService<TransientTestService>();
            var transientTestService2 = lazyServiceProvider.LazyGetRequiredService<TransientTestService>();
            transientTestService1.ShouldBeSameAs(transientTestService2);

            var testCounter = application.ServiceProvider.GetRequiredService<ITestCounter>();
            testCounter.GetValue(nameof(TransientTestService)).ShouldBe(1);
        }
    }

    [DependsOn(typeof(AbpTestBaseModule))]
    private class TestModule : AbpModule
    {
        public TestModule()
        {
            SkipAutoServiceRegistration = true;
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddType<TransientTestService>();
        }
    }

    private class TransientTestService : ITransientDependency
    {
        public TransientTestService(ITestCounter counter)
        {
            counter.Increment(nameof(TransientTestService));
        }
    }
}
