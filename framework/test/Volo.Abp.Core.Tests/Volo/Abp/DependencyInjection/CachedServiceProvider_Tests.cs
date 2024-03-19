using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Testing.Utils;
using Xunit;

namespace Volo.Abp.DependencyInjection;

public class CachedServiceProvider_Tests
{
    [Fact]
    public void CachedServiceProvider_Should_Cache_Services()
    {
        void TestResolvingServices(IServiceScope scope)
        {
            var cachedServiceProvider1 = scope.ServiceProvider.GetRequiredService<ICachedServiceProvider>();
            var cachedServiceProvider2 = scope.ServiceProvider.GetRequiredService<ICachedServiceProvider>();
            cachedServiceProvider1.ShouldBeSameAs(cachedServiceProvider2);

            var transientTestService1 = cachedServiceProvider1.GetRequiredService<TransientTestService>();
            var transientTestService2 = cachedServiceProvider2.GetRequiredService<TransientTestService>();
            transientTestService1.ShouldBeSameAs(transientTestService2);

            var transientKeyedTestService1 = cachedServiceProvider1.GetRequiredKeyedService<TransientKeyedTestService>("key1");
            var transientKeyedTestService2 = cachedServiceProvider2.GetRequiredKeyedService<TransientKeyedTestService>("key1");
            transientKeyedTestService1.ShouldBeSameAs(transientKeyedTestService2);

            var cachedServiceProvider3 = cachedServiceProvider1.GetRequiredService<IServiceProvider>().GetRequiredService<ICachedServiceProvider>();
            cachedServiceProvider3.ShouldBeSameAs(cachedServiceProvider1);
        }

        using (var application = AbpApplicationFactory.Create<TestModule>())
        {
            application.Initialize();

            using (var scope1 = application.ServiceProvider.CreateScope())
            {
                TestResolvingServices(scope1);
                var testCounter = scope1.ServiceProvider.GetRequiredService<ITestCounter>();
                testCounter.GetValue(nameof(TransientTestService)).ShouldBe(1);
            }

            using (var scope2 = application.ServiceProvider.CreateScope())
            {
                TestResolvingServices(scope2);
                var testCounter = scope2.ServiceProvider.GetRequiredService<ITestCounter>();

                //Resolved in a different scope, so should not cache the service!
                testCounter.GetValue(nameof(TransientTestService)).ShouldBe(2);
            }
        }
    }

    [Fact]
    public void TransientCachedServiceProvider_Should_Cache_Services()
    {
        void TestResolvingServices(IServiceScope scope)
        {
            var transientCachedServiceProvider1 = scope.ServiceProvider.GetRequiredService<ITransientCachedServiceProvider>();

            var transientTestService1_1 = transientCachedServiceProvider1.GetRequiredService<TransientTestService>();
            var transientTestService1_2 = transientCachedServiceProvider1.GetRequiredService<TransientTestService>();
            transientTestService1_1.ShouldBeSameAs(transientTestService1_2);

            var transientKeyedTestService1 = transientCachedServiceProvider1.GetRequiredKeyedService<TransientKeyedTestService>("key1");
            var transientKeyedTestService2 = transientCachedServiceProvider1.GetRequiredKeyedService<TransientKeyedTestService>("key1");
            transientKeyedTestService1.ShouldBeSameAs(transientKeyedTestService2);
            var transientKeyedTestService3 = transientCachedServiceProvider1.GetRequiredKeyedService<TransientKeyedTestService>("key2");
            transientKeyedTestService2.ShouldNotBeSameAs(transientKeyedTestService3);

            var transientCachedServiceProvider2 = scope.ServiceProvider.GetRequiredService<ITransientCachedServiceProvider>();
            transientCachedServiceProvider1.ShouldNotBeSameAs(transientCachedServiceProvider2);

            var transientTestService2_1 = transientCachedServiceProvider2.GetRequiredService<TransientTestService>();
            var transientTestService2_2 = transientCachedServiceProvider2.GetRequiredService<TransientTestService>();
            transientTestService2_1.ShouldBeSameAs(transientTestService2_2);

            var transientKeyedTestService1_1 = transientCachedServiceProvider2.GetRequiredKeyedService<TransientKeyedTestService>("key1");
            var transientKeyedTestService2_1 = transientCachedServiceProvider2.GetRequiredKeyedService<TransientKeyedTestService>("key1");
            transientKeyedTestService1_1.ShouldBeSameAs(transientKeyedTestService2_1);

            transientTestService1_1.ShouldNotBeSameAs(transientTestService2_1);
            transientKeyedTestService1.ShouldNotBeSameAs(transientKeyedTestService1_1);

            var transientCachedServiceProvider1_1 = transientCachedServiceProvider1.GetRequiredService<IServiceProvider>().GetRequiredService<ITransientCachedServiceProvider>();
            var transientCachedServiceProvider1_2 = transientCachedServiceProvider1.GetRequiredService<IServiceProvider>().GetRequiredService<ITransientCachedServiceProvider>();
            transientCachedServiceProvider1_1.ShouldNotBeSameAs(transientCachedServiceProvider1);
            transientCachedServiceProvider1_2.ShouldNotBeSameAs(transientCachedServiceProvider1);
            transientCachedServiceProvider1_1.ShouldNotBeSameAs(transientCachedServiceProvider1_2);
        }

        using (var application = AbpApplicationFactory.Create<TestModule>())
        {
            application.Initialize();

            using (var scope1 = application.ServiceProvider.CreateScope())
            {
                TestResolvingServices(scope1);
                var testCounter = scope1.ServiceProvider.GetRequiredService<ITestCounter>();
                testCounter.GetValue(nameof(TransientTestService)).ShouldBe(2);
            }
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
            context.Services.AddKeyedTransient<TransientKeyedTestService>("key1");
            context.Services.AddKeyedTransient<TransientKeyedTestService>("key2");
        }
    }

    private class TransientTestService : ITransientDependency
    {
        public TransientTestService(ITestCounter counter)
        {
            counter.Increment(nameof(TransientTestService));
        }
    }

    private class TransientKeyedTestService
    {

    }
}
