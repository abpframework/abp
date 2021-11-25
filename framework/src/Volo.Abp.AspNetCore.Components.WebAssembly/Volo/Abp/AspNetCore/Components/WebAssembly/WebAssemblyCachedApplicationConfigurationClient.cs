using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ClientProxies;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

[ExposeServices(
    typeof(WebAssemblyCachedApplicationConfigurationClient),
    typeof(ICachedApplicationConfigurationClient),
    typeof(IAsyncInitialize)
    )]
public class WebAssemblyCachedApplicationConfigurationClient : ICachedApplicationConfigurationClient, ITransientDependency
{
    protected AbpApplicationConfigurationClientProxy ApplicationConfigurationAppService { get; }

    protected ApplicationConfigurationCache Cache { get; }

    protected ICurrentTenantAccessor CurrentTenantAccessor { get; }

    public WebAssemblyCachedApplicationConfigurationClient(
        AbpApplicationConfigurationClientProxy applicationConfigurationAppService,
        ApplicationConfigurationCache cache,
        ICurrentTenantAccessor currentTenantAccessor)
    {
        ApplicationConfigurationAppService = applicationConfigurationAppService;
        Cache = cache;
        CurrentTenantAccessor = currentTenantAccessor;
    }

    public virtual async Task InitializeAsync()
    {
        var configurationDto = await ApplicationConfigurationAppService.GetAsync();

        Cache.Set(configurationDto);

        CurrentTenantAccessor.Current = new BasicTenantInfo(
            configurationDto.CurrentTenant.Id,
            configurationDto.CurrentTenant.Name
        );
    }

    public virtual Task<ApplicationConfigurationDto> GetAsync()
    {
        return Task.FromResult(GetConfigurationByChecking());
    }

    public virtual ApplicationConfigurationDto Get()
    {
        return GetConfigurationByChecking();
    }

    private ApplicationConfigurationDto GetConfigurationByChecking()
    {
        var configuration = Cache.Get();
        if (configuration == null)
        {
            throw new AbpException(
                $"{nameof(WebAssemblyCachedApplicationConfigurationClient)} should be initialized before using it.");
        }

        return configuration;
    }
}
