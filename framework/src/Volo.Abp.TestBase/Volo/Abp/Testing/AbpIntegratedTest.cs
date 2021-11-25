using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Testing;

public abstract class AbpIntegratedTest<TStartupModule> : AbpTestBaseWithServiceProvider, IDisposable
    where TStartupModule : IAbpModule
{
    protected IAbpApplication Application { get; }

    protected override IServiceProvider ServiceProvider => Application.ServiceProvider;

    protected IServiceProvider RootServiceProvider { get; }

    protected IServiceScope TestServiceScope { get; }

    protected AbpIntegratedTest()
    {
        var services = CreateServiceCollection();

        BeforeAddApplication(services);

        var application = services.AddApplication<TStartupModule>(SetAbpApplicationCreationOptions);
        Application = application;

        AfterAddApplication(services);

        RootServiceProvider = CreateServiceProvider(services);
        TestServiceScope = RootServiceProvider.CreateScope();

        application.Initialize(TestServiceScope.ServiceProvider);
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

    public virtual void Dispose()
    {
        Application.Shutdown();
        TestServiceScope.Dispose();
        Application.Dispose();
    }
}
