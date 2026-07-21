using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ClientProxies;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Threading;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class MvcCachedApplicationConfigurationClient : ICachedApplicationConfigurationClient, ITransientDependency
{
    private const string HttpContextItemsCacheKeyFormat = "ApplicationConfigurationDto_{0}_{1}_CacheKey";

    protected IHttpContextAccessor HttpContextAccessor { get; }
    protected AbpApplicationConfigurationClientProxy ApplicationConfigurationAppService { get; }
    protected AbpApplicationLocalizationClientProxy ApplicationLocalizationClientProxy { get; }
    protected ICurrentUser CurrentUser { get; }
    protected MvcCachedApplicationConfigurationClientHelper CacheHelper { get; }
    protected IDistributedCache<ApplicationConfigurationDto> Cache { get; }
    protected AbpAspNetCoreMvcClientCacheOptions Options { get; }

    public MvcCachedApplicationConfigurationClient(
        MvcCachedApplicationConfigurationClientHelper cacheHelper,
        IDistributedCache<ApplicationConfigurationDto> cache,
        AbpApplicationConfigurationClientProxy applicationConfigurationAppService,
        ICurrentUser currentUser,
        IHttpContextAccessor httpContextAccessor,
        AbpApplicationLocalizationClientProxy applicationLocalizationClientProxy,
        IOptions<AbpAspNetCoreMvcClientCacheOptions> options)
    {
        ApplicationConfigurationAppService = applicationConfigurationAppService;
        CurrentUser = currentUser;
        HttpContextAccessor = httpContextAccessor;
        ApplicationLocalizationClientProxy = applicationLocalizationClientProxy;
        Options = options.Value;
        CacheHelper = cacheHelper;
        Cache = cache;
    }

    public virtual async Task<ApplicationConfigurationDto> GetAsync()
    {
        string? cacheKey = null;
        var httpContext = HttpContextAccessor?.HttpContext;
        var itemsKey = GetHttpContextItemsCacheKey();
        if (httpContext != null && httpContext.Items[itemsKey] is string key)
        {
            cacheKey = key;
        }

        if (cacheKey.IsNullOrWhiteSpace())
        {
            cacheKey = await CreateCacheKeyAsync();
            if (httpContext != null)
            {
                httpContext.Items[itemsKey] = cacheKey;
            }
        }

        if (httpContext != null && httpContext.Items[cacheKey] is ApplicationConfigurationDto configuration)
        {
            return configuration;
        }

        configuration = (await Cache.GetOrAddAsync(
            cacheKey,
            async () => await GetRemoteConfigurationAsync(),
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = Options.ApplicationConfigurationDtoCacheAbsoluteExpiration
            }
        ))!;

        if (httpContext != null)
        {
            httpContext.Items[cacheKey] = configuration;
        }

        return configuration;
    }

    protected virtual async Task<ApplicationConfigurationDto> GetRemoteConfigurationAsync()
    {
        var cultureName = CultureInfo.CurrentUICulture.Name;

        var configTask = ApplicationConfigurationAppService.GetAsync(
            new ApplicationConfigurationRequestOptions
            {
                IncludeLocalizationResources = false
            }
        );

        var localizationTask = ApplicationLocalizationClientProxy.GetAsync(
            new ApplicationLocalizationRequestDto
            {
                CultureName = cultureName,
                OnlyDynamics = true
            }
        );

        await Task.WhenAll(configTask, localizationTask);

        var config = configTask.Result;
        var localizationDto = localizationTask.Result;

        if (!CultureHelper.IsCompatibleCulture(config.Localization.CurrentCulture.Name, cultureName))
        {
            localizationDto = await ApplicationLocalizationClientProxy.GetAsync(
                new ApplicationLocalizationRequestDto
                {
                    CultureName = config.Localization.CurrentCulture.Name,
                    OnlyDynamics = true
                }
            );
        }

        config.Localization.Resources = localizationDto.Resources;

        return config;
    }

    public virtual ApplicationConfigurationDto Get()
    {
        string? cacheKey = null;
        var httpContext = HttpContextAccessor?.HttpContext;
        var itemsKey = GetHttpContextItemsCacheKey();
        if (httpContext != null && httpContext.Items[itemsKey] is string key)
        {
            cacheKey = key;
        }

        if (cacheKey.IsNullOrWhiteSpace())
        {
            cacheKey = AsyncHelper.RunSync(CreateCacheKeyAsync);
            if (httpContext != null)
            {
                httpContext.Items[itemsKey] = cacheKey;
            }
        }

        if (httpContext != null && httpContext.Items[cacheKey] is ApplicationConfigurationDto configuration)
        {
            return configuration;
        }

        return AsyncHelper.RunSync(GetAsync);
    }

    protected virtual async Task<string> CreateCacheKeyAsync()
    {
        return await CacheHelper.CreateCacheKeyAsync(CurrentUser.Id);
    }

    protected virtual string GetHttpContextItemsCacheKey()
    {
        return string.Format(CultureInfo.InvariantCulture, HttpContextItemsCacheKeyFormat, CurrentUser.Id?.ToString("N") ?? "Anonymous", CultureInfo.CurrentUICulture.Name);
    }
}
