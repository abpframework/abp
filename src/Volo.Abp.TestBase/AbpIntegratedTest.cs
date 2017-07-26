using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.TestBase
{
    public class AbpIntegratedTest<TStartupModule> : IDisposable
        where TStartupModule : IAbpModule
    {
        protected IAbpApplication Application { get; }

        protected IServiceProvider ServiceProvider => Application.ServiceProvider;

        protected IServiceScope MainServiceScope { get; }

        public AbpIntegratedTest()
        {
            var services = CreateServiceCollection();

            BeforeAddApplication(services);

            var application = services.AddApplication<TStartupModule>(SetAbpApplicationCreationOptions);
            Application = application;

            AfterAddApplication(services);

            MainServiceScope = CreateServiceProvider(services).CreateScope();
            var serviceProvider = MainServiceScope.ServiceProvider;

            application.Initialize(serviceProvider);
        }

        protected virtual IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
        }

        protected virtual void BeforeAddApplication(IServiceCollection services)
        {

        }

        protected virtual void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {

        }

        protected virtual void AfterAddApplication(IServiceCollection services)
        {

        }

        protected virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
	        return services.BuildServiceProviderFromFactory();
        }

        public void Dispose()
        {
            Application.Shutdown();
            MainServiceScope.Dispose();
            Application.Dispose();
        }
    }
}
