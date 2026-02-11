using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;

namespace Volo.Abp.Caching;

public class AbpDistributedCacheOptions
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
    public DistributedCacheEntryOptions GlobalCacheEntryOptions { get; set; }

    /// <summary>
    /// List of all cache configurators.
    /// (func argument:Name of cache)
    /// </summary>
    public List<Func<string, DistributedCacheEntryOptions?>> CacheConfigurators { get; set; } //TODO: use a configurator interface instead?

    public AbpDistributedCacheOptions()
    {
        CacheConfigurators = new List<Func<string, DistributedCacheEntryOptions?>>();
        GlobalCacheEntryOptions = new DistributedCacheEntryOptions();
        KeyPrefix = "";
    }

    public void ConfigureCache<TCacheItem>(DistributedCacheEntryOptions? options)
    {
        ConfigureCache(typeof(TCacheItem), options);
    }
    
    public void ConfigureCache(Type cacheItemType, DistributedCacheEntryOptions? options)
    {
        ConfigureCache(CacheNameAttribute.GetCacheName(cacheItemType), options);
    }
    
    public void ConfigureCache(string cacheName, DistributedCacheEntryOptions? options)
    {
        CacheConfigurators.Add(name => cacheName != name ? null : options);
    }
}
