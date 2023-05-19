using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.ProxyScripting;

public class ProxyScriptManagerCache : IProxyScriptManagerCache, ISingletonDependency
{
    private readonly ConcurrentDictionary<string, string> _cache;

    public ProxyScriptManagerCache()
    {
        _cache = new ConcurrentDictionary<string, string>();
    }

    public string GetOrAdd(string key, Func<string> factory)
    {
        return _cache.GetOrAdd(key, factory);
    }

    public void Set(string key, string value)
    {
        _cache[key] = value;
    }
}
