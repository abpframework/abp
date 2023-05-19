using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class BundleCache : IBundleCache, ISingletonDependency
{
    private readonly ConcurrentDictionary<string, BundleCacheItem> _cache;

    public BundleCache()
    {
        _cache = new ConcurrentDictionary<string, BundleCacheItem>();
    }

    public BundleCacheItem GetOrAdd(string bundleName, Func<BundleCacheItem> factory)
    {
        return _cache.GetOrAdd(bundleName, factory);
    }

    public bool Remove(string bundleName)
    {
        return _cache.TryRemove(bundleName, out _);
    }
}
