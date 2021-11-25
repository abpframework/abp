using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Volo.Abp.Caching;

public interface ICacheSupportsMultipleItems
{
    byte[][] GetMany(
        IEnumerable<string> keys
    );

    Task<byte[][]> GetManyAsync(
        IEnumerable<string> keys,
        CancellationToken token = default
    );

    void SetMany(
        IEnumerable<KeyValuePair<string, byte[]>> items,
        DistributedCacheEntryOptions options
    );

    Task SetManyAsync(
        IEnumerable<KeyValuePair<string, byte[]>> items,
        DistributedCacheEntryOptions options,
        CancellationToken token = default
    );

    void RefreshMany(
        IEnumerable<string> keys
    );

    Task RefreshManyAsync(
        IEnumerable<string> keys,
        CancellationToken token = default
    );

    void RemoveMany(
        IEnumerable<string> keys
    );

    Task RemoveManyAsync(
        IEnumerable<string> keys,
        CancellationToken token = default
    );
}
