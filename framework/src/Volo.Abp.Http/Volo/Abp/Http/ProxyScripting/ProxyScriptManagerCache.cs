using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.ProxyScripting;

public class ProxyScriptManagerCache : IProxyScriptManagerCache, ISingletonDependency
{
    private readonly ConcurrentDictionary<string, string> _cache = new();
    private readonly ConcurrentDictionary<string, Lazy<Task<string>>> _asyncCache = new();

    public async Task<string> GetOrAddAsync(string key, Func<Task<string>> factory)
    {
        if (_cache.TryGetValue(key, out var cached))
        {
            return cached;
        }

        var result = await _asyncCache.GetOrAdd(
            key,
            _ => new Lazy<Task<string>>(factory, LazyThreadSafetyMode.ExecutionAndPublication)
        ).Value;

        _cache[key] = result;
        _asyncCache.TryRemove(key, out _);

        return result;
    }
}
