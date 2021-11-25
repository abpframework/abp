namespace Volo.Abp.Caching;

public class DistributedCacheKeyNormalizeArgs
{
    public string Key { get; }

    public string CacheName { get; }

    public bool IgnoreMultiTenancy { get; }

    public DistributedCacheKeyNormalizeArgs(
        string key,
        string cacheName,
        bool ignoreMultiTenancy)
    {
        Key = key;
        CacheName = cacheName;
        IgnoreMultiTenancy = ignoreMultiTenancy;
    }
}
