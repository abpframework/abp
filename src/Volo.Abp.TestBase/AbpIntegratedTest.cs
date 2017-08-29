using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using System.Threading.Tasks;

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


        protected virtual void UseUnitOfWork(Action action)
        {
            using (IServiceScope scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin())
                {
                    action();

                    uow.Complete();
                }
            }
        }

        protected virtual async Task UseUnitOfWorkAsync(Func<Task> action)
        {
            using (IServiceScope scope = ServiceProvider.CreateScope())
            {
                var uowManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

                using (var uow = uowManager.Begin())
                {
                    await action();

                    await uow.CompleteAsync();
                }
            }
        }

        public void Dispose()
        {
            Application.Shutdown();
            MainServiceScope.Dispose();
            Application.Dispose();
        }
    }
}
