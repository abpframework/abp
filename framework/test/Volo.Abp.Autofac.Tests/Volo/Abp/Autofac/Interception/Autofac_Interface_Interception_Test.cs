using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Builder;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DynamicProxy;
using Volo.Abp.TestBase.Logging;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Autofac.Interception;

public class Autofac_Interface_Interception_Test : AbpAsyncIntegratedTest<AutofacTestModule>, IAsyncLifetime
{
    protected override Task<Action<AbpApplicationCreationOptions>> SetAbpApplicationCreationOptionsAsync()
    {
        return Task.FromResult<Action<AbpApplicationCreationOptions>>(options => options.UseAutofac());
    }

    protected override Task BeforeAddApplicationAsync(IServiceCollection services)
    {
        services.AddTransient<SimpleAsyncInterceptor>();
        services.AddTransient<IInterfaceInterceptionTarget, InterfaceInterceptionTarget>();
        services.OnRegistered(registration =>
        {
            if (typeof(InterfaceInterceptionTarget) == registration.ImplementationType)
            {
                registration.Interceptors.Add<SimpleAsyncInterceptor>();
            }
        });

        return Task.CompletedTask;
    }

    [Fact]
    public async Task Should_Intercept_A_Service_Registered_By_Its_Interface()
    {
        var target = GetRequiredService<IInterfaceInterceptionTarget>();

        ProxyUtil.IsProxy(target).ShouldBeTrue();
        (await target.GetValueAsync()).ShouldBe(42);
        // Read from the target itself: reading Logs through the proxy is intercepted too.
        ((InterfaceInterceptionTarget)ProxyUtil.GetUnproxiedInstance(target)).Logs.ShouldBe(new[]
        {
            "SimpleAsyncInterceptor_InterceptAsync_BeforeInvocation",
            "ExecutingGetValueAsync",
            "SimpleAsyncInterceptor_InterceptAsync_AfterInvocation"
        });
    }

    [Fact]
    public void Should_Generate_The_Proxy_With_The_Sharded_Generator()
    {
        var target = GetRequiredService<IInterfaceInterceptionTarget>();

        var targetType = typeof(InterfaceInterceptionTarget);
        var additionalInterfaces = targetType.GetInterfaces()
            .Where(ProxyUtil.IsAccessible)
            .Where(i => i != typeof(IInterfaceInterceptionTarget))
            .ToArray();
        var proxyType = AbpInterceptionRegistrationExtensions.ProxyGenerator.ProxyBuilder.CreateInterfaceProxyTypeWithTarget(
            typeof(IInterfaceInterceptionTarget), additionalInterfaces, targetType, ProxyGenerationOptions.Default);

        // The same key returns the cached type only from the generator that built the resolved proxy.
        target.GetType().ShouldBeSameAs(proxyType);
    }

    public interface IInterfaceInterceptionTarget : ICanLogOnObject
    {
        Task<int> GetValueAsync();
    }

    public class InterfaceInterceptionTarget : IInterfaceInterceptionTarget
    {
        public List<string> Logs { get; } = new();

        public Task<int> GetValueAsync()
        {
            Logs.Add("ExecutingGetValueAsync");
            return Task.FromResult(42);
        }
    }
}
