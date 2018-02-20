using System;
using System.Collections.Generic;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.TestBase;
using Xunit;

namespace Microsoft.Extensions.DependencyInjection
{
    public class DependencyInjection_Standard_Tests : AbpIntegratedTest<DependencyInjection_Standard_Tests.TestModule>
    {
        [Fact]
        public void Singleton_Service_Should_Resolve_Dependencies_Independent_From_The_Scope()
        {
            MySingletonService singletonService;
            MyTransientService2 transientService2;

            using (var scope = ServiceProvider.CreateScope())
            {
                var transientService1 = scope.ServiceProvider.GetRequiredService<MyTransientService1>();
                transientService2 = scope.ServiceProvider.GetRequiredService<MyTransientService2>();

                transientService1.DoIt();
                transientService1.DoIt();

                singletonService = transientService1.SingletonService;
                singletonService.TransientInstances.Count.ShouldBe(2);
            }

            Assert.Equal(singletonService, GetRequiredService<MySingletonService>());

            singletonService.TransientInstances.Count.ShouldBe(2);
            singletonService.TransientInstances.ForEach(ts => ts.IsDisposed.ShouldBeFalse());

            transientService2.IsDisposed.ShouldBeTrue();
        }

        public class MySingletonService : ISingletonDependency
        {
            public List<MyTransientService2> TransientInstances { get; }

            public IServiceProvider ServiceProvider { get; }

            public MySingletonService(IServiceProvider serviceProvider)
            {
                ServiceProvider = serviceProvider;
                TransientInstances = new List<MyTransientService2>();
            }

            public void ResolveTransient()
            {
                TransientInstances.Add(
                    ServiceProvider.GetRequiredService<MyTransientService2>()
                );
            }
        }

        public class MyTransientService1 : ITransientDependency
        {
            public MySingletonService SingletonService { get; }

            public MyTransientService1(MySingletonService singletonService)
            {
                SingletonService = singletonService;
            }

            public void DoIt()
            {
                SingletonService.ResolveTransient();
            }
        }

        public class MyTransientService2 : ITransientDependency, IDisposable
        {
            public bool IsDisposed { get; set; }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        public class TestModule : AbpModule
        {
            public override void ConfigureServices(IServiceCollection services)
            {
                services.AddType<MySingletonService>();
                services.AddType<MyTransientService1>();
                services.AddType<MyTransientService2>();
            }
        }
    }
}
