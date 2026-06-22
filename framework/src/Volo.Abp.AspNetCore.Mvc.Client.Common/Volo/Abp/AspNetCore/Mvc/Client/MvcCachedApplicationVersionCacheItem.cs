namespace Volo.Abp.AspNetCore.Mvc.Client;

public class MvcCachedApplicationVersionCacheItem
{
    public const string CacheKey = "Mvc_Application_Version";

    public string Version { get; set; }

    public MvcCachedApplicationVersionCacheItem(string version)
    {
        Version = version;
    }
}
