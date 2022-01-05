using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.ObjectMapping;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using ApiScope = Volo.Abp.IdentityServer.ApiScopes.ApiScope;
using IdentityResource = Volo.Abp.IdentityServer.IdentityResources.IdentityResource;

namespace Volo.Abp.IdentityServer;

public class ResourceStore : IResourceStore
{
    public const string AllResourcesKey = "AllResources";
    public const string ApiResourceNameCacheKeyPrefix = "ApiResourceName_";
    public const string ApiResourceScopeNameCacheKeyPrefix = "ApiResourceScopeName_";

    protected IIdentityResourceRepository IdentityResourceRepository { get; }
    protected IApiResourceRepository ApiResourceRepository { get; }
    protected IApiScopeRepository ApiScopeRepository { get; }
    protected IObjectMapper<AbpIdentityServerDomainModule> ObjectMapper { get; }
    protected IDistributedCache<IdentityServer4.Models.IdentityResource> IdentityResourceCache { get; }
    protected IDistributedCache<IdentityServer4.Models.ApiScope> ApiScopeCache { get; }
    protected IDistributedCache<IdentityServer4.Models.ApiResource> ApiResourceCache { get; }
    protected IDistributedCache<IdentityServer4.Models.Resources> ResourcesCache { get; }
    protected IdentityServerOptions Options { get; }

    public ResourceStore(
        IIdentityResourceRepository identityResourceRepository,
        IObjectMapper<AbpIdentityServerDomainModule> objectMapper,
        IApiResourceRepository apiResourceRepository,
        IApiScopeRepository apiScopeRepository,
        IDistributedCache<IdentityServer4.Models.IdentityResource> identityResourceCache,
        IDistributedCache<IdentityServer4.Models.ApiScope> apiScopeCache,
        IDistributedCache<IdentityServer4.Models.ApiResource> apiResourceCache,
        IDistributedCache<Resources> resourcesCache,
        IOptions<IdentityServerOptions> options)
    {
        IdentityResourceRepository = identityResourceRepository;
        ObjectMapper = objectMapper;
        ApiResourceRepository = apiResourceRepository;
        ApiScopeRepository = apiScopeRepository;
        IdentityResourceCache = identityResourceCache;
        ApiScopeCache = apiScopeCache;
        ApiResourceCache = apiResourceCache;
        ResourcesCache = resourcesCache;
        Options = options.Value;
    }

    /// <summary>
    /// Gets identity resources by scope name.
    /// </summary>
    public virtual async Task<IEnumerable<IdentityServer4.Models.IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return await GetCacheItemsAsync(
            IdentityResourceCache,
            scopeNames,
            async keys => await IdentityResourceRepository.GetListByScopeNameAsync(keys, includeDetails: true),
            (models, cacheKeyPrefix) => new List<IEnumerable<KeyValuePair<string, IdentityServer4.Models.IdentityResource>>>
            {
                    models.Select(x => new KeyValuePair<string, IdentityServer4.Models.IdentityResource>(AddCachePrefix(x.Name, cacheKeyPrefix), x))
            });
    }

    /// <summary>
    /// Gets API scopes by scope name.
    /// </summary>
    public virtual async Task<IEnumerable<IdentityServer4.Models.ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        return await GetCacheItemsAsync(
            ApiScopeCache,
            scopeNames,
            async keys => await ApiScopeRepository.GetListByNameAsync(keys, includeDetails: true),
            (models, cacheKeyPrefix) => new List<IEnumerable<KeyValuePair<string, IdentityServer4.Models.ApiScope>>>
            {
                    models.Select(x => new KeyValuePair<string, IdentityServer4.Models.ApiScope>(AddCachePrefix(x.Name, cacheKeyPrefix), x))
            });
    }

    /// <summary>
    /// Gets API resources by scope name.
    /// </summary>
    public virtual async Task<IEnumerable<IdentityServer4.Models.ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return await GetCacheItemsAsync<ApiResource, IdentityServer4.Models.ApiResource>(
            ApiResourceCache,
            scopeNames,
            async keys => await ApiResourceRepository.GetListByScopesAsync(keys, includeDetails: true),
            (models, cacheKeyPrefix) =>
            {
                return models
                    .Select(model => model.Scopes.Select(scope => new KeyValuePair<string, IdentityServer4.Models.ApiResource>(AddCachePrefix(scope, cacheKeyPrefix), model)).ToList())
                    .Where(scopes => scopes.Any()).Cast<IEnumerable<KeyValuePair<string, IdentityServer4.Models.ApiResource>>>().ToList();
            }, ApiResourceScopeNameCacheKeyPrefix);
    }

    /// <summary>
    /// Gets API resources by API resource name.
    /// </summary>
    public virtual async Task<IEnumerable<IdentityServer4.Models.ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        return await GetCacheItemsAsync(
            ApiResourceCache,
            apiResourceNames,
            async keys => await ApiResourceRepository.FindByNameAsync(keys, includeDetails: true),
            (models, cacheKeyPrefix) => new List<IEnumerable<KeyValuePair<string, IdentityServer4.Models.ApiResource>>>
            {
                    models.Select(x => new KeyValuePair<string, IdentityServer4.Models.ApiResource>(AddCachePrefix(x.Name, cacheKeyPrefix), x))
            }, ApiResourceNameCacheKeyPrefix);
    }

    /// <summary>
    /// Gets all resources.
    /// </summary>
    public virtual async Task<IdentityServer4.Models.Resources> GetAllResourcesAsync()
    {
        return await ResourcesCache.GetOrAddAsync(AllResourcesKey, async () =>
        {
            var identityResources = await IdentityResourceRepository.GetListAsync(includeDetails: true);
            var apiResources = await ApiResourceRepository.GetListAsync(includeDetails: true);
            var apiScopes = await ApiScopeRepository.GetListAsync(includeDetails: true);

            return new Resources(
                ObjectMapper.Map<List<Volo.Abp.IdentityServer.IdentityResources.IdentityResource>, List<IdentityServer4.Models.IdentityResource>>(identityResources),
                ObjectMapper.Map<List<Volo.Abp.IdentityServer.ApiResources.ApiResource>, List<IdentityServer4.Models.ApiResource>>(apiResources),
                ObjectMapper.Map<List<Volo.Abp.IdentityServer.ApiScopes.ApiScope>, List<IdentityServer4.Models.ApiScope>>(apiScopes));
        }, () => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = Options.Caching.ClientStoreExpiration
        });
    }

    protected virtual async Task<IEnumerable<TModel>> GetCacheItemsAsync<TEntity, TModel>(
        IDistributedCache<TModel> cache,
        IEnumerable<string> keys,
        Func<string[], Task<List<TEntity>>> entityFactory,
        Func<List<TModel>, string, List<IEnumerable<KeyValuePair<string, TModel>>>> cacheItemsFactory,
        string cacheKeyPrefix = null)
        where TModel : class
    {
        var cacheItems = await cache.GetManyAsync(AddCachePrefix(keys, cacheKeyPrefix));
        if (cacheItems.All(x => x.Value != null))
        {
            return cacheItems.Select(x => x.Value);
        }

        var otherKeys = RemoveCachePrefix(cacheItems.Where(x => x.Value == null).Select(x => x.Key), cacheKeyPrefix).ToArray();
        var otherModels = ObjectMapper.Map<List<TEntity>, List<TModel>>(await entityFactory(otherKeys));
        var otherCacheItems = cacheItemsFactory(otherModels, cacheKeyPrefix).ToList();
        foreach (var item in otherCacheItems)
        {
            await cache.SetManyAsync(item, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = Options.Caching.ClientStoreExpiration
            });
        }

        return cacheItems.Where(x => x.Value != null).Select(x => x.Value).Concat(otherModels);
    }

    protected virtual IEnumerable<string> AddCachePrefix(IEnumerable<string> keys, string prefix)
    {
        return prefix == null ? keys : keys.Select(x => AddCachePrefix(x, prefix));
    }

    protected virtual string AddCachePrefix(string key, string prefix)
    {
        return prefix == null ? key : prefix + key;
    }

    protected virtual IEnumerable<string> RemoveCachePrefix(IEnumerable<string> keys, string prefix)
    {
        return prefix == null ? keys : keys.Select(x => RemoveCachePrefix(x, prefix));
    }

    protected virtual string RemoveCachePrefix(string key, string prefix)
    {
        return prefix == null ? key : key.RemovePreFix(prefix);
    }
}
