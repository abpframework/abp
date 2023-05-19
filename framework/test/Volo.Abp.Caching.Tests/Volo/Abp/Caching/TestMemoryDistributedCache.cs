using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Caching;

[DisableConventionalRegistration]
public class TestMemoryDistributedCache : MemoryDistributedCache, ICacheSupportsMultipleItems
{
    public TestMemoryDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor)
        : base(optionsAccessor)
    {
    }

    public TestMemoryDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor, ILoggerFactory loggerFactory)
        : base(optionsAccessor, loggerFactory)
    {
    }

    public byte[][] GetMany(IEnumerable<string> keys)
    {
        var values = new List<byte[]>();
        foreach (var key in keys)
        {
            values.Add(Get(key));
        }
        return values.ToArray();
    }

    public async Task<byte[][]> GetManyAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        var values = new List<byte[]>();
        foreach (var key in keys)
        {
            values.Add(await GetAsync(key, token));
        }
        return values.ToArray();
    }

    public void SetMany(IEnumerable<KeyValuePair<string, byte[]>> items, DistributedCacheEntryOptions options)
    {
        foreach (var item in items)
        {
            Set(item.Key, item.Value, options);
        }
    }

    public async Task SetManyAsync(IEnumerable<KeyValuePair<string, byte[]>> items, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        foreach (var item in items)
        {
            await SetAsync(item.Key, item.Value, options, token);
        }
    }

    public void RefreshMany(IEnumerable<string> keys)
    {
        foreach (var key in keys)
        {
            Refresh(key);
        }
    }

    public async Task RefreshManyAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        foreach (var key in keys)
        {
            await RefreshAsync(key, token);
        }
    }

    public void RemoveMany(IEnumerable<string> keys)
    {
        foreach (var key in keys)
        {
            Remove(key);
        }
    }

    public async Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        foreach (var key in keys)
        {
            await RemoveAsync(key, token);
        }
    }
}
