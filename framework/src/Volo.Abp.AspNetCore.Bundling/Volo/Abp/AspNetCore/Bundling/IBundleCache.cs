using System;

namespace Volo.Abp.AspNetCore.Bundling;

public interface IBundleCache
{
    BundleCacheItem GetOrAdd(string bundleName, Func<BundleCacheItem> factory);

    bool Remove(string bundleName);
}
