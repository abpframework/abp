using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Autofac;

public class AutofacRegistration_Tests : AbpIntegratedTest<AutofacRegistration_Tests.TestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Fact]
    public void Should_Resolve_AnyKey_Keyed_Service_With_Any_Key()
    {
        // AnyKey registration should be resolvable with any key value.
        var serviceWithKeyA = GetRequiredKeyedService<IAnyKeyService>("keyA");
        var serviceWithKeyB = GetRequiredKeyedService<IAnyKeyService>("keyB");
        var serviceWithKeyC = GetRequiredKeyedService<IAnyKeyService>(42);

        serviceWithKeyA.ShouldNotBeNull();
        serviceWithKeyB.ShouldNotBeNull();
        serviceWithKeyC.ShouldNotBeNull();

        serviceWithKeyA.ShouldBeOfType<AnyKeyServiceImpl>();
        serviceWithKeyB.ShouldBeOfType<AnyKeyServiceImpl>();
        serviceWithKeyC.ShouldBeOfType<AnyKeyServiceImpl>();
    }

    [Fact]
    public void Should_Pass_Correct_Key_To_Keyed_Factory()
    {
        var serviceA = GetRequiredKeyedService<IKeyedFactoryService>("alpha");
        var serviceB = GetRequiredKeyedService<IKeyedFactoryService>("beta");

        serviceA.Key.ShouldBe("alpha");
        serviceB.Key.ShouldBe("beta");
    }

    [Fact]
    public void Should_Not_Dispose_Instance_Registration_When_Scope_Disposed()
    {
        // Resolve the pre-registered singleton instance.
        var instance = GetRequiredKeyedService<IDisposableInstance>("instance");
        instance.ShouldNotBeNull();
        instance.IsDisposed.ShouldBeFalse();

        // The same instance should be returned from a child scope.
        using (var scope = ServiceProvider.CreateScope())
        {
            var scopedInstance = scope.ServiceProvider.GetRequiredKeyedService<IDisposableInstance>("instance");
            scopedInstance.ShouldBeSameAs(instance);
        }

        // After the scope is disposed, the singleton instance should still be alive.
        instance.IsDisposed.ShouldBeFalse();

        // It should also be the same static instance registered in the module.
        instance.ShouldBeSameAs(TestModule.DisposableInstanceForTest);
    }

    [Fact]
    public void Should_Resolve_Standard_Keyed_Services()
    {
        var big = GetRequiredKeyedService<ITypedCache>("big");
        var small = GetRequiredKeyedService<ITypedCache>("small");

        big.ShouldBeOfType<BigTypedCache>();
        small.ShouldBeOfType<SmallTypedCache>();

        big.Get("test").ShouldBe("big:test");
        small.Get("test").ShouldBe("small:test");
    }

    [DependsOn(typeof(AbpAutofacModule))]
    public class TestModule : AbpModule
    {
        public static DisposableInstance DisposableInstanceForTest { get; } = new();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // AnyKey registration: this service can be resolved with any key.
            context.Services.AddKeyedTransient<IAnyKeyService, AnyKeyServiceImpl>(
                Microsoft.Extensions.DependencyInjection.KeyedService.AnyKey);

            // Keyed factory registration: the factory receives the actual key used for resolution.
            context.Services.Add(ServiceDescriptor.KeyedTransient<IKeyedFactoryService>(
                Microsoft.Extensions.DependencyInjection.KeyedService.AnyKey,
                (sp, key) => new KeyedFactoryServiceImpl(key)));

            // Instance registration with keyed service (ExternallyOwned should prevent Autofac from disposing it).
            context.Services.AddKeyedSingleton<IDisposableInstance>("instance", DisposableInstanceForTest);

            // Standard keyed type registrations.
            context.Services.AddKeyedTransient<ITypedCache, BigTypedCache>("big");
            context.Services.AddKeyedTransient<ITypedCache, SmallTypedCache>("small");
        }
    }

    public interface IAnyKeyService
    {
    }

    public class AnyKeyServiceImpl : IAnyKeyService
    {
    }

    public interface IKeyedFactoryService
    {
        object Key { get; }
    }

    public class KeyedFactoryServiceImpl : IKeyedFactoryService
    {
        public object Key { get; }

        public KeyedFactoryServiceImpl(object key)
        {
            Key = key;
        }
    }

    public interface IDisposableInstance
    {
        bool IsDisposed { get; }
    }

    public class DisposableInstance : IDisposableInstance, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    public interface ITypedCache
    {
        string Get(string key);
    }

    public class BigTypedCache : ITypedCache
    {
        public string Get(string key) => $"big:{key}";
    }

    public class SmallTypedCache : ITypedCache
    {
        public string Get(string key) => $"small:{key}";
    }
}
