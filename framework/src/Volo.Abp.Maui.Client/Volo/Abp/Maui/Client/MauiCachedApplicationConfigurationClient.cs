using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ClientProxies;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.Maui.Client;

public class MauiCachedApplicationConfigurationClient :
    ICachedApplicationConfigurationClient,
    ISingletonDependency
{
    protected AbpApplicationConfigurationClientProxy ApplicationConfigurationClientProxy { get; }
    protected AbpApplicationLocalizationClientProxy ApplicationLocalizationClientProxy { get; }
    protected ApplicationConfigurationCache Cache { get; }
    protected ICurrentTenantAccessor CurrentTenantAccessor { get; }

    public MauiCachedApplicationConfigurationClient(
        AbpApplicationConfigurationClientProxy applicationConfigurationClientProxy,
        AbpApplicationLocalizationClientProxy applicationLocalizationClientProxy,
        ApplicationConfigurationCache cache,
        ICurrentTenantAccessor currentTenantAccessor)
    {
        ApplicationConfigurationClientProxy = applicationConfigurationClientProxy;
        ApplicationLocalizationClientProxy = applicationLocalizationClientProxy;
        CurrentTenantAccessor = currentTenantAccessor;
        Cache = cache;
    }

    public virtual async Task<ApplicationConfigurationDto> InitializeAsync()
    {
        var configurationDto = await ApplicationConfigurationClientProxy.GetAsync(
            new ApplicationConfigurationRequestOptions
            {
                IncludeLocalizationResources = false,
            });

        var localizationDto = await ApplicationLocalizationClientProxy.GetAsync(
            new ApplicationLocalizationRequestDto
            {
                CultureName = configurationDto.Localization.CurrentCulture.Name,
                OnlyDynamics = true
            }
        );

        configurationDto.Localization.Resources = localizationDto.Resources;

        CurrentTenantAccessor.Current = new BasicTenantInfo(
            configurationDto.CurrentTenant.Id,
            configurationDto.CurrentTenant.Name);

        Cache.Set(configurationDto);

        return configurationDto;
    }

    public virtual ApplicationConfigurationDto Get()
    {
        return AsyncHelper.RunSync(GetAsync);
    }

    public virtual async Task<ApplicationConfigurationDto> GetAsync()
    {
        var configurationDto = Cache.Get();
        if (configurationDto is null)
        {
            return await InitializeAsync();
        }

        return configurationDto;
    }
}
