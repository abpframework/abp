using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.TestBase
{
    public class AbpIntegratedTest<TStartupModule> : IDisposable
        where TStartupModule : IAbpModule
    {
        protected AbpApplication Application { get; }

        protected IServiceProvider ServiceProvider => Application.ServiceProvider;

        protected IServiceScope MainServiceScope { get; private set; }

        public AbpIntegratedTest()
        {
            var services = CreateServiceCollection();

            BeforeAddApplication(services);

            Application = services.AddApplication<TStartupModule>();

            AfterAddApplication(services);

            var serviceProvider = CreateServiceProvider(services);

            Application.Initialize(serviceProvider);
        }

        protected virtual IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
        }

        protected virtual void BeforeAddApplication(IServiceCollection services)
        {

        }

        protected virtual void AfterAddApplication(IServiceCollection services)
        {

        }

        protected virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            MainServiceScope = services.BuildServiceProvider().CreateScope();
            return MainServiceScope.ServiceProvider;
        }

        public void Dispose()
        {
            Application.Shutdown();
            MainServiceScope.Dispose();
        }
    }
}
