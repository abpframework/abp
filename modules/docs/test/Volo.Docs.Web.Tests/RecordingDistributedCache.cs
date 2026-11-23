using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Volo.Docs;

public sealed class RecordingDistributedCache : IDistributedCache
{
    private readonly MemoryDistributedCache _inner = new(Options.Create(new MemoryDistributedCacheOptions()));
    public ConcurrentQueue<string> Writes { get; } = new();

    public byte[] Get(string key) => _inner.Get(key);
    public Task<byte[]> GetAsync(string key, CancellationToken token = default) => _inner.GetAsync(key, token);
    public void Refresh(string key) => _inner.Refresh(key);
    public Task RefreshAsync(string key, CancellationToken token = default) => _inner.RefreshAsync(key, token);
    public void Remove(string key) => _inner.Remove(key);
    public Task RemoveAsync(string key, CancellationToken token = default) => _inner.RemoveAsync(key, token);

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        Writes.Enqueue(key);
        _inner.Set(key, value, options);
    }

    public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        Writes.Enqueue(key);
        return _inner.SetAsync(key, value, options, token);
    }

}
