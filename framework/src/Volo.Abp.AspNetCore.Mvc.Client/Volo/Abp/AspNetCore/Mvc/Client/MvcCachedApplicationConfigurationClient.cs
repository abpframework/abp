using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ClientProxies;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class MvcCachedApplicationConfigurationClient : ICachedApplicationConfigurationClient, ITransientDependency
{
    protected IHttpContextAccessor HttpContextAccessor { get; }
    protected AbpApplicationConfigurationClientProxy ApplicationConfigurationAppService { get; }
    protected AbpApplicationLocalizationClientProxy ApplicationLocalizationClientProxy { get; }
    protected ICurrentUser CurrentUser { get; }
    protected IDistributedCache<ApplicationConfigurationDto> Cache { get; }

    public MvcCachedApplicationConfigurationClient(
        IDistributedCache<ApplicationConfigurationDto> cache,
        AbpApplicationConfigurationClientProxy applicationConfigurationAppService,
        ICurrentUser currentUser,
        IHttpContextAccessor httpContextAccessor, 
        AbpApplicationLocalizationClientProxy applicationLocalizationClientProxy)
    {
        ApplicationConfigurationAppService = applicationConfigurationAppService;
        CurrentUser = currentUser;
        HttpContextAccessor = httpContextAccessor;
        ApplicationLocalizationClientProxy = applicationLocalizationClientProxy;
        Cache = cache;
    }

    public async Task<ApplicationConfigurationDto> GetAsync()
    {
        var cacheKey = CreateCacheKey();
        var httpContext = HttpContextAccessor?.HttpContext;

        if (httpContext != null && httpContext.Items[cacheKey] is ApplicationConfigurationDto configuration)
        {
            return configuration;
        }

        configuration = await Cache.GetOrAddAsync(
            cacheKey,
            async () => await GetRemoteConfigurationAsync(),
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300) //TODO: Should be configurable.
            }
        );

        if (httpContext != null)
        {
            httpContext.Items[cacheKey] = configuration;
        }

        return configuration;
    }

    private async Task<ApplicationConfigurationDto> GetRemoteConfigurationAsync()
    {
        var config = await ApplicationConfigurationAppService.GetAsync(
            new ApplicationConfigurationRequestOptions
            {
                IncludeLocalizationResources = false
            }
        );
        
        var localizationDto = await ApplicationLocalizationClientProxy.GetAsync(
            new ApplicationLocalizationRequestDto {
                CultureName = config.Localization.CurrentCulture.Name,
                OnlyDynamics = true
            }
        );

        config.Localization.Resources = localizationDto.Resources;
        
        return config;
    }

    public ApplicationConfigurationDto Get()
    {
        var cacheKey = CreateCacheKey();
        var httpContext = HttpContextAccessor?.HttpContext;

        if (httpContext != null && httpContext.Items[cacheKey] is ApplicationConfigurationDto configuration)
        {
            return configuration;
        }

        return AsyncHelper.RunSync(GetAsync);
    }

    protected virtual string CreateCacheKey()
    {
        return MvcCachedApplicationConfigurationClientHelper.CreateCacheKey(CurrentUser);
    }
}
