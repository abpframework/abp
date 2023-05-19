using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public interface IBundleCache
{
    BundleCacheItem GetOrAdd(string bundleName, Func<BundleCacheItem> factory);

    bool Remove(string bundleName);
}
