using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Hybrid;

namespace Volo.Abp.Caching.Hybrid;

public class AbpHybridCacheOptions
{
    /// <summary>
    /// Throw or hide exceptions for the distributed cache.
    /// </summary>
    public bool HideErrors { get; set; } = true;

    /// <summary>
    /// Cache key prefix.
    /// </summary>
    public string KeyPrefix { get; set; }

    /// <summary>
    /// Global Cache entry options.
    /// </summary>
    public HybridCacheEntryOptions GlobalHybridCacheEntryOptions { get; set; }

    /// <summary>
    /// List of all cache configurators.
    /// (func argument:Name of cache)
    /// </summary>
    public List<Func<string, HybridCacheEntryOptions?>> CacheConfigurators { get; set; } //TODO: use a configurator interface instead?

    public AbpHybridCacheOptions()
    {
        CacheConfigurators = new List<Func<string, HybridCacheEntryOptions?>>();
        GlobalHybridCacheEntryOptions = new HybridCacheEntryOptions();
        KeyPrefix = "";
    }

    public void ConfigureCache<TCacheItem>(HybridCacheEntryOptions? options)
    {
        ConfigureCache(typeof(TCacheItem), options);
    }

    public void ConfigureCache(Type cacheItemType, HybridCacheEntryOptions? options)
    {
        ConfigureCache(CacheNameAttribute.GetCacheName(cacheItemType), options);
    }

    public void ConfigureCache(string cacheName, HybridCacheEntryOptions? options)
    {
        CacheConfigurators.Add(name => cacheName != name ? null : options);
    }
}
