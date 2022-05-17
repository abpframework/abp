using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Testing;

public class AbpAsyncIntegratedTest<TStartupModule> : AbpTestBaseWithServiceProvider
    where TStartupModule : IAbpModule
{
    protected IAbpApplication Application { get; set; }

    protected IServiceProvider RootServiceProvider { get; set; }

    protected IServiceScope TestServiceScope { get; set; }

    public virtual async Task InitializeAsync()
    {
        var services = await CreateServiceCollectionAsync();

        await BeforeAddApplicationAsync(services);
        var application = await services.AddApplicationAsync<TStartupModule>(await SetAbpApplicationCreationOptionsAsync());
        await AfterAddApplicationAsync(services);

        RootServiceProvider = await CreateServiceProviderAsync(services);
        TestServiceScope = RootServiceProvider.CreateScope();
        await application.InitializeAsync(TestServiceScope.ServiceProvider);
        ServiceProvider = application.ServiceProvider;
        Application = application;

        await InitializeServicesAsync();
    }

    public virtual async Task DisposeAsync()
    {
        await Application.ShutdownAsync();
        TestServiceScope.Dispose();
        Application.Dispose();
    }

    protected virtual Task<IServiceCollection> CreateServiceCollectionAsync()
    {
        return Task.FromResult<IServiceCollection>(new ServiceCollection());
    }

    protected virtual Task BeforeAddApplicationAsync(IServiceCollection services)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<Action<AbpApplicationCreationOptions>> SetAbpApplicationCreationOptionsAsync()
    {
        return Task.FromResult<Action<AbpApplicationCreationOptions>>(_ => { });
    }

    protected virtual Task AfterAddApplicationAsync(IServiceCollection services)
    {
        return Task.CompletedTask;
    }

    protected virtual Task<IServiceProvider> CreateServiceProviderAsync(IServiceCollection services)
    {
        return Task.FromResult(services.BuildServiceProviderFromFactory());
    }

    protected virtual Task InitializeServicesAsync()
    {
        return Task.CompletedTask;
    }
}
