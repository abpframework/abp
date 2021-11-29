using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.DependencyInjection;

public class DependencyInjection_Tests
{
    [Fact]
    public void Singletons_Should_Resolve_Transients_Independent_From_Current_Scope()
    {
        //Arrange

        var services = new ServiceCollection();

        services
            .AddSingleton<MySingletonServiceUsesTransients>()
            .AddTransient<MyTransientServiceUsesSingleton>()
            .AddTransient<MyTransientService>();

        MySingletonServiceUsesTransients singletonService;

        using (var serviceProvider = services.BuildServiceProvider())
        {
            //Act

            using (var scope = serviceProvider.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<MyTransientServiceUsesSingleton>().DoIt();
                scope.ServiceProvider.GetRequiredService<MyTransientServiceUsesSingleton>().DoIt();
            }

            using (var scope = serviceProvider.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<MyTransientServiceUsesSingleton>().DoIt();
                scope.ServiceProvider.GetRequiredService<MyTransientServiceUsesSingleton>().DoIt();
                scope.ServiceProvider.GetRequiredService<MySingletonServiceUsesTransients>().ShouldNotBeDisposed();
            }

            singletonService = serviceProvider.GetRequiredService<MySingletonServiceUsesTransients>();
            singletonService.ShouldNotBeDisposed();
        }

        singletonService.ShouldBeDisposed();
    }

    [Fact]
    public void Should_Release_Resolved_Services_When_Main_Service_Is_Disposed()
    {
        var services = new ServiceCollection();

        services
            .AddTransient<MyTransientServiceUsesTransients>()
            .AddTransient<MyTransientService>();

        using (var serviceProvider = services.BuildServiceProvider())
        {
            MyTransientServiceUsesTransients myTransientServiceUsesTransients;

            using (var scope = serviceProvider.CreateScope())
            {
                myTransientServiceUsesTransients = scope.ServiceProvider.GetRequiredService<MyTransientServiceUsesTransients>();

                myTransientServiceUsesTransients.DoIt();
                myTransientServiceUsesTransients.DoIt();

                myTransientServiceUsesTransients.ShouldNotBeDisposed();
            }

            myTransientServiceUsesTransients.ShouldBeDisposed();
        }
    }

    [Fact]
    public void Inner_Scope_Should_Resolve_New_Scoped_Service()
    {
        var services = new ServiceCollection();

        services
            .AddScoped<ScopedServiceWithState>();

        using (var serviceProvider = services.BuildServiceProvider())
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var service1 = scope.ServiceProvider.GetRequiredService<ScopedServiceWithState>();
                var service2 = scope.ServiceProvider.GetRequiredService<ScopedServiceWithState>();

                service1.ShouldBe(service2);

                using (var innerScope = scope.ServiceProvider.CreateScope())
                {
                    var innserService1 = innerScope.ServiceProvider.GetRequiredService<ScopedServiceWithState>();
                    var innserService2 = innerScope.ServiceProvider.GetRequiredService<ScopedServiceWithState>();

                    innserService1.ShouldBe(innserService2);
                    innserService1.ShouldNotBe(service1);
                }
            }
        }
    }

    private class MyTransientServiceUsesSingleton
    {
        private readonly MySingletonServiceUsesTransients _singletonService;

        public MyTransientServiceUsesSingleton(MySingletonServiceUsesTransients singletonService)
        {
            _singletonService = singletonService;
        }

        public void DoIt()
        {
            _singletonService.DoIt();
        }
    }

    private class MySingletonServiceUsesTransients
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly List<MyTransientService> _instances;

        public MySingletonServiceUsesTransients(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _instances = new List<MyTransientService>();
        }

        public void DoIt()
        {
            _instances.Add(_serviceProvider.GetRequiredService<MyTransientService>());
        }

        public void ShouldNotBeDisposed()
        {
            foreach (var instance in _instances)
            {
                instance.IsDisposed.ShouldBeFalse();
            }
        }

        public void ShouldBeDisposed()
        {
            foreach (var instance in _instances)
            {
                instance.IsDisposed.ShouldBeTrue();
            }
        }
    }

    private class MyTransientService : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    private class MyTransientServiceUsesTransients
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly List<MyTransientService> _instances;

        public MyTransientServiceUsesTransients(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _instances = new List<MyTransientService>();
        }

        public void DoIt()
        {
            _instances.Add(_serviceProvider.GetRequiredService<MyTransientService>());
        }

        public void ShouldNotBeDisposed()
        {
            foreach (var instance in _instances)
            {
                instance.IsDisposed.ShouldBeFalse();
            }
        }

        public void ShouldBeDisposed()
        {
            foreach (var instance in _instances)
            {
                instance.IsDisposed.ShouldBeTrue();
            }
        }
    }

    private class ScopedServiceWithState
    {
        private readonly Dictionary<string, object> _items;

        public ScopedServiceWithState()
        {
            _items = new Dictionary<string, object>();
        }

        public void Set(string name, object value)
        {
            _items[name] = value;
        }

        public object Get(string name)
        {
            return _items[name];
        }
    }
}
