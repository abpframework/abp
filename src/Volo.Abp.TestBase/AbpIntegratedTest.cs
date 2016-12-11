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

        public AbpIntegratedTest()
        {
            var services = CreateServiceCollection();
            Application = AbpApplication.Create<TStartupModule>(services);
            var serviceProvider = CreateServiceProvider(services);
            Application.Initialize(serviceProvider);
        }

        protected virtual IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
        }

        protected virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        public void Dispose()
        {
            Application.Dispose();
        }
    }
}
