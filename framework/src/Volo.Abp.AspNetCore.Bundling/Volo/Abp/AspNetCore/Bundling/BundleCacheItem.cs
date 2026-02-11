using System;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Bundling;

public class BundleCacheItem
{
    public List<BundleFile> Files { get; }

    public List<IDisposable> WatchDisposeHandles { get; }

    public BundleCacheItem(List<BundleFile> files)
    {
        Files = files;
        WatchDisposeHandles = new List<IDisposable>();
    }
}
