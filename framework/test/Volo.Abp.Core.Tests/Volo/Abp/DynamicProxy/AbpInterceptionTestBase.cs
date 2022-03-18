using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.DynamicProxy;

public abstract class AbpInterceptionTestBase<TStartupModule> : AbpAsyncIntegratedTest<TStartupModule>, IAsyncLifetime
    where TStartupModule : IAbpModule
{
    protected override Task BeforeAddApplicationAsync(IServiceCollection services)
    {
        services.AddTransient<SimpleAsyncInterceptor>();
        services.AddTransient<SimpleAsyncInterceptor2>();
        services.AddTransient<SimpleInterceptionTargetClass>();

        services.AddTransient<SimpleResultCacheTestInterceptor>();
        services.AddTransient<CachedTestObject>();

        services.OnRegistred(registration =>
        {
            if (typeof(SimpleInterceptionTargetClass) == registration.ImplementationType)
            {
                registration.Interceptors.Add<SimpleAsyncInterceptor>();
                registration.Interceptors.Add<SimpleAsyncInterceptor2>();
            }

            if (typeof(CachedTestObject) == registration.ImplementationType)
            {
                registration.Interceptors.Add<SimpleResultCacheTestInterceptor>();
            }
        });

        return Task.CompletedTask;
    }

    [Fact]
    public async Task Should_Intercept_Async_Method_Without_Return_Value()
    {
        //Arrange

        var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

        //Act

        await target.DoItAsync();

        //Assert

        target.Logs.Count.ShouldBe(7);
        target.Logs[0].ShouldBe("SimpleAsyncInterceptor_InterceptAsync_BeforeInvocation");
        target.Logs[1].ShouldBe("SimpleAsyncInterceptor2_InterceptAsync_BeforeInvocation");
        target.Logs[2].ShouldBe("EnterDoItAsync");
        target.Logs[3].ShouldBe("MiddleDoItAsync");
        target.Logs[4].ShouldBe("ExitDoItAsync");
        target.Logs[5].ShouldBe("SimpleAsyncInterceptor2_InterceptAsync_AfterInvocation");
        target.Logs[6].ShouldBe("SimpleAsyncInterceptor_InterceptAsync_AfterInvocation");
    }

    [Fact]
    public async Task Should_Intercept_Async_Method_With_Return_Value()
    {
        //Arrange

        var target = ServiceProvider.GetService<SimpleInterceptionTargetClass>();

        //Act

        var result = await target.GetValueAsync();

        //Assert

        result.ShouldBe(42);
        target.Logs.Count.ShouldBe(7);
        target.Logs[0].ShouldBe("SimpleAsyncInterceptor_InterceptAsync_BeforeInvocation");
        target.Logs[1].ShouldBe("SimpleAsyncInterceptor2_InterceptAsync_BeforeInvocation");
        target.Logs[2].ShouldBe("EnterGetValueAsync");
        target.Logs[3].ShouldBe("MiddleGetValueAsync");
        target.Logs[4].ShouldBe("ExitGetValueAsync");
        target.Logs[5].ShouldBe("SimpleAsyncInterceptor2_InterceptAsync_AfterInvocation");
        target.Logs[6].ShouldBe("SimpleAsyncInterceptor_InterceptAsync_AfterInvocation");
    }

    [Fact]
    public async Task Should_Cache_Results_Async()
    {
        //Arrange

        var target = ServiceProvider.GetService<CachedTestObject>();

        //Act & Assert

        (await target.GetValueAsync(42)).ShouldBe(42); //First run, not cached yet
        (await target.GetValueAsync(43)).ShouldBe(42); //First run, cached previous value
        (await target.GetValueAsync(44)).ShouldBe(42); //First run, cached previous value
    }
}
