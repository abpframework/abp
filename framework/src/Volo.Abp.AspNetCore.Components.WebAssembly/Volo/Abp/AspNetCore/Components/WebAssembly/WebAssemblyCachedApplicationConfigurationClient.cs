﻿using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ClientProxies;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public class WebAssemblyCachedApplicationConfigurationClient : ICachedApplicationConfigurationClient, ITransientDependency
{
    protected AbpApplicationConfigurationClientProxy ApplicationConfigurationClientProxy { get; }
    
    protected AbpApplicationLocalizationClientProxy ApplicationLocalizationClientProxy { get; }

    protected ApplicationConfigurationCache Cache { get; }

    protected ICurrentTenantAccessor CurrentTenantAccessor { get; }

    public WebAssemblyCachedApplicationConfigurationClient(
        AbpApplicationConfigurationClientProxy applicationConfigurationClientProxy,
        ApplicationConfigurationCache cache,
        ICurrentTenantAccessor currentTenantAccessor, 
        AbpApplicationLocalizationClientProxy applicationLocalizationClientProxy)
    {
        ApplicationConfigurationClientProxy = applicationConfigurationClientProxy;
        Cache = cache;
        CurrentTenantAccessor = currentTenantAccessor;
        ApplicationLocalizationClientProxy = applicationLocalizationClientProxy;
    }

    public virtual async Task InitializeAsync()
    {
        var configurationDto = await ApplicationConfigurationClientProxy.GetAsync(
            new ApplicationConfigurationRequestOptions {
                IncludeLocalizationResources = false
            }
        );

        var localizationDto = await ApplicationLocalizationClientProxy.GetAsync(
            new ApplicationLocalizationRequestDto {
                CultureName = configurationDto.Localization.CurrentCulture.Name,
                OnlyDynamics = true
            }
        );

        configurationDto.Localization.Resources = localizationDto.Resources;
        
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
