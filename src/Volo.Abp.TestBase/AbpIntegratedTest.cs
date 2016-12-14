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

            Application = services.AddApplication<TStartupModule>();

            var serviceProvider = CreateServiceProvider(services);

            Application.Initialize(serviceProvider);
        }

        protected virtual IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
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
