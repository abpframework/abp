using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.DependencyInjection
{
    public class DependencyInjection_Tests
    {
        [Fact]
        public void Singletons_Should_Resolve_Transients_Independent_From_Current_Scope()
        {
            //Arrange

            var services = new ServiceCollection();

            services
                .AddSingleton<MySingletonService>()
                .AddTransient<MyTransientServiceUsesSingleton>()
                .AddTransient<MyTransientService>();

            MySingletonService singletonService;

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
                    scope.ServiceProvider.GetRequiredService<MySingletonService>().ShouldNotBeDisposed();
                }

                singletonService = serviceProvider.GetRequiredService<MySingletonService>();
                singletonService.ShouldNotBeDisposed();
            }

            singletonService.ShouldBeDisposed();
        }

        private class MyTransientServiceUsesSingleton
        {
            private readonly MySingletonService _singletonService;

            public MyTransientServiceUsesSingleton(MySingletonService singletonService)
            {
                _singletonService = singletonService;
            }

            public void DoIt()
            {
                _singletonService.DoIt();
            }
        }

        private class MySingletonService
        {
            private readonly IServiceProvider _serviceProvider;

            private readonly List<MyTransientService> _instances;

            public MySingletonService(IServiceProvider serviceProvider)
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
    }
}
